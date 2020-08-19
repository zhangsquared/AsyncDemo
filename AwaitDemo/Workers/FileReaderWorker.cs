using System.Collections.Generic;
using System.Threading.Tasks;

namespace AwaitDemo
{
    public class FileReaderWorker
    {
        private readonly FileReader reader;
        private readonly int loop = 5;

        public FileReaderWorker(FileReader reader)
        {
            this.reader = reader;
        }

        public void Work()
        {
            for (int i = 0; i < loop; i++)
            {
                reader.Read(i);
            }
        }

        public async Task WorkAsync()
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < loop; i++)
            {
                tasks.Add(reader.ReadAsync(i));
            }
            await Task.WhenAll(tasks.ToArray());
        }

        public async Task WorkTaskAsync()
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < loop; i++)
            {
                tasks.Add(reader.ReadTaskAsync(i));
            }
            await Task.WhenAll(tasks.ToArray());
        }
    }
}
