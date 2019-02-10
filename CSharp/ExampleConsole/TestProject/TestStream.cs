using System;
using System.IO;

namespace TestProject
{
    public class TestStream : Stream
    {
        private readonly TestStreamWriter _testWriter;
        private bool _canRead;

        public override bool CanRead
        {
            get
            {
                return _canRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override long Length
        {
            get
            {
                return (long)this._testWriter.Count;
            }
        }

        public override long Position
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public TestStream()
        {
            this._testWriter = new TestStreamWriter();
            this._canRead = true;
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int num = this._testWriter.Read(buffer, count);
            if (num < count)
                _canRead = false;

            return num;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
