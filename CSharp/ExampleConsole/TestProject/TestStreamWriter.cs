using System;
using System.Collections.Concurrent;

namespace TestProject
{
    // pre-defined stream, always has only one string
    internal class TestStreamWriter : IDisposable
    {
        private const string DATA = "The quick brown fox jumps over the lazy dog's back 1234567890.";
        private readonly BlockingCollection<byte> _byteQueue = new BlockingCollection<byte>();

        internal long Count
        {
            get
            {
                return this._byteQueue.Count;
            }
        }

        internal TestStreamWriter()
        {
            this.PopulateQueue();
        }

        private static byte[] GetBytes(string str)
        {
            byte[] buffer = new byte[str.Length * 2];
            Buffer.BlockCopy((Array)str.ToCharArray(), 0, (Array)buffer, 0, buffer.Length);
            return buffer;
        }

        private byte[] GetBuffer()
        {
            return TestStreamWriter.GetBytes(string.Join(". ", DATA));
        }

        private void PopulateQueue()
        {
            foreach (byte num in this.GetBuffer())
                this._byteQueue.Add(num);
        }

        internal int Read(byte[] buffer, int count)
        {
            int position = 0;

            for (int index = 0; index < count; index++)
            {
                if (this._byteQueue.Count <= 0)
                    return position;

                byte currentItem;

                if (this._byteQueue.TryTake(out currentItem))
                {
                    buffer[index] = currentItem;
                    position++;
                }
                else
                {
                    break;
                }
            }
            return position;
        }

        public void Dispose()
        {
            _byteQueue.Dispose();
        }
    }
}
