using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TemplateWizard;
using EnvDTE;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;


namespace Accela.Mobile.CustomWizard
{
    public class Wizard : IWizard
    {
        private frmUserInput inputForm;
        private string certificateSubjectName;
        private Task certCreationTask;
        private byte[] certBlob;
        private string packageCertificateKeyFile;


        // This method is called before opening any item that 
        // has the OpenInEditor attribute.
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
            if (project == null)
            {
                throw new System.ArgumentNullException();
            }
            try
            {
                this.packageCertificateKeyFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(project.FullName), project.Name + "_TemporaryKey.pfx");
            }
            catch (System.Exception ex)
            {
                if (Wizard.IsCriticalException(ex))
                {
                    throw;
                }
            }
        }

        // This method is only called for item templates,
        // not for project templates.
        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        // This method is called after the project is created.
        public void RunFinished()
        {
            try
            {
                if (!this.certCreationTask.Wait(System.TimeSpan.FromSeconds(10.0)))
                {
                    Trace.WriteLine("Project Wizard: Unable to create certificate. Creating certificate timed out.");
                }
                else
                {
                    if (string.IsNullOrEmpty(this.packageCertificateKeyFile) || this.certBlob == null)
                    {
                        Trace.WriteLine("Project Wizard: Unable to create certificate. Unable to determine key file name or create key.");
                    }
                    else
                    {
                        System.IO.File.WriteAllBytes(this.packageCertificateKeyFile, this.certBlob);
                    }
                }
            }
            catch (System.Exception ex)
            {
                if (Wizard.IsCriticalException(ex))
                {
                    throw;
                }
                Trace.WriteLine(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Project Wizard: Unable to create certificate. Failed with Exception of type '{0}' with message '{1}'.", new object[]
				{
					ex.GetType(),
					ex.Message
				}));
            }
            finally
            {
                this.certCreationTask = null;
                this.certBlob = null;
                this.packageCertificateKeyFile = null;
            }
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                inputForm = new frmUserInput();

#if DEBUG
                inputForm.SetReplacementsDictionary(replacementsDictionary);
#endif


                if (inputForm.ShowDialog() != DialogResult.OK)
                    throw new WizardBackoutException();

                replacementsDictionary.Add("$appid$", inputForm.AppId);
                replacementsDictionary.Add("$appsecret$", inputForm.AppSecret);

                //Microsoft.VisualStudio.WinRT.TemplateWizards.CreateProjectCertificate.Wizard
                string text;
                if (!replacementsDictionary.TryGetValue("$username$", out text))
                {
                    Trace.WriteLine("Microsoft.VisualStudio.WinRT.TemplateWizards.CreateProjectCertificate.Wizard: Unable to transform the current $username$ to a valid certificate subject common name.");
                    throw new ArgumentNullException();
                }
                this.certificateSubjectName = CryptoHelper.CreateSubjectFromPublisherName(text);
                string value = System.Security.SecurityElement.Escape(this.certificateSubjectName);
                string value2 = System.Security.SecurityElement.Escape(text);
                replacementsDictionary.Add("$XmlEscapedPublisherDistinguishedName$", value);
                replacementsDictionary.Add("$XmlEscapedPublisher$", value2);
                replacementsDictionary.Add("$includeKeyFile$", "true");
                this.certCreationTask = Task.Factory.StartNew(delegate
                {
                    this.certBlob = CryptoHelper.CreateX509CertificateBlob(this.certificateSubjectName, 2048u, null);
                });

                return;
            }
            catch (WizardBackoutException)
            {
                if (replacementsDictionary["$exclusiveproject$"] == true.ToString())
                    Directory.Delete(replacementsDictionary["$solutiondirectory$"], true);
                else
                    Directory.Delete(replacementsDictionary["$destinationdirectory$"], true);
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error occurred running wizard:\n\n{0}", ex));
                throw new WizardCancelledException("Internal error", ex);
            }
        }

        // This method is only called for item templates,
        // not for project templates.
        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        private static bool IsCriticalException(Exception ex)
        {
            return ex is StackOverflowException || ex is OutOfMemoryException || ex is ThreadAbortException || ex is AccessViolationException;
        }
    }
}
