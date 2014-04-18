using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace Accela.WindowsStoreSDK
{
    //public class AccelaSDKProgressStream : Stream
    //{
    //    private StorageFile _file;
    //    private long _RWCount = 0;
    //    private long _writeLength = 0;
    //    private Stream _baseStream;
    //    private CancellationTokenSource _cancellationTokenSource;

    //    public delegate void ProgressHandler(object sender, long Progress, long Transfered, long Length, StorageFile file);
    //    public event ProgressHandler Progress;

    //    private void UpdateProgress(long ProgressPercentage, long Transfered, long Length, StorageFile file)
    //    {
    //        ProgressHandler handler = Progress;
    //        if (handler != null)
    //        {
    //            // Invokes the delegates.
    //            handler(this, ProgressPercentage, Transfered, Length, file);
    //        }
    //    }

    //    public AccelaSDKProgressStream(Stream baseStream, StorageFile file)
    //    {
    //        _baseStream = baseStream;
    //        _file = file;
    //    }

    //    public AccelaSDKProgressStream(Stream baseStream, StorageFile file, long writeLength)
    //    {
    //        _baseStream = baseStream;
    //        _file = file;
    //        _writeLength = writeLength;
    //    }

    //    public AccelaSDKProgressStream(Stream baseStream, StorageFile file, CancellationTokenSource cancellationTokenSource)
    //    {
    //        _baseStream = baseStream;
    //        _file = file;
    //        _cancellationTokenSource = cancellationTokenSource;
    //    }

    //    public AccelaSDKProgressStream(Stream baseStream, StorageFile file, long writeLength, CancellationTokenSource cancellationTokenSource)
    //    {
    //        _baseStream = baseStream;
    //        _file = file;
    //        _writeLength = writeLength;
    //        _cancellationTokenSource = cancellationTokenSource;
    //    }

    //    #region override

    //    public override bool CanRead
    //    {
    //        get { return _baseStream.CanRead; }
    //    }

    //    public override bool CanSeek
    //    {
    //        get { return _baseStream.CanSeek; }
    //    }

    //    public override bool CanWrite
    //    {
    //        get { return _baseStream.CanWrite; }
    //    }

    //    public override void Flush()
    //    {
    //        _baseStream.Flush();
    //    }

    //    public override long Length
    //    {
    //        get { return _baseStream.Length; }
    //    }

    //    public override long Position
    //    {
    //        get
    //        {
    //            return _baseStream.Position;
    //        }
    //        set
    //        {
    //            _baseStream.Position = value;
    //        }
    //    }

    //    public override int Read(byte[] buffer, int offset, int count)
    //    {
    //        if (_cancellationTokenSource != null && _cancellationTokenSource.IsCancellationRequested) 
    //            throw new TaskCanceledException();

    //        int read = _baseStream.Read(buffer, offset, count);

    //        _RWCount += read;
            
    //        long percent = _RWCount * 100 / _baseStream.Length;
            
    //        UpdateProgress(percent, _RWCount, _baseStream.Length, _file);

    //        return read;
    //    }

    //    public override long Seek(long offset, SeekOrigin origin)
    //    {
    //        return _baseStream.Seek(offset, origin);
    //    }

    //    public override void SetLength(long value)
    //    {
    //        _baseStream.SetLength(value);
    //    }

    //    public override void Write(byte[] buffer, int offset, int count)
    //    {
    //        if (_cancellationTokenSource != null && _cancellationTokenSource.IsCancellationRequested) 
    //            throw new TaskCanceledException();

    //        _baseStream.Write(buffer, offset, count);

    //        if (_writeLength > 0)
    //        {
    //            //throw new Exception("Total file length required by constructor when writing stream");
    //            _RWCount += count;
    //            long percent = _RWCount * 100 / _writeLength;
    //            UpdateProgress(percent, _RWCount, _writeLength, _file);
    //        }
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        _baseStream.Dispose();
    //        base.Dispose(disposing);
    //    }

    //    #endregion
    //}
}
