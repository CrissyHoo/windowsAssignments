using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsumerandProducer
{
    class Program
    {
        //内部类，可通过外层类访问,方便对产品做标记
        class Item
        {
            public string name = "一个产品";
        }

        // 产品队列缓存
        static Queue<Item> queue = new Queue<Item>();
        static readonly int BUFFER_SIZE = 5;

        // 同步标记
        private static int ItemCount = 0;
        static Semaphore fillCount = new Semaphore(0, BUFFER_SIZE);
        static Semaphore emptyCount = new Semaphore(BUFFER_SIZE, BUFFER_SIZE);
        static Mutex bufferMutex = new Mutex();

        // 将产品放入缓存中
        static void putItemIntoBuffer(Item item)
        {
            queue.Enqueue(item);
            Console.WriteLine("将" + item.name + "放入队列，仓库现在有" + queue.Count + "个");
        }

        // 从缓存中获取产品
        static Item removeItemFromBuffer()
        {
            var item = queue.Peek();//返回队列开始处的对象但不移除
            queue.Dequeue();
            Console.WriteLine("将" + item.name + "取出队列，现在有" + queue.Count + "个");
            return item;
        }

        // 生产产品
        static Item ProduceItem(int number)
        {
            Thread.Sleep(1000);
            Item item = new Item() { name = "产品" + number };
            Console.WriteLine("生产了" + item.name);
            return item;
        }

        // 消费产品
        static void ConsumItem(Item item)
        {
            Thread.Sleep(4000);
            Console.WriteLine("消费了" + item.name);
        }

        static Thread producerThread;
        static Thread consumerThread;

        static void producer1()
        {
            int productNumber = 0;
            while (true)
            {
                var item = ProduceItem(productNumber++);

                // 还有空的位置的话，就可以继续生产
                emptyCount.WaitOne();
                //获取缓冲区的互斥权限
                bufferMutex.WaitOne();
                // 将产品放入buffer中
                putItemIntoBuffer(item);
                bufferMutex.ReleaseMutex();
                // 生产成功了一个空的，所以产品占位置数加一
                fillCount.Release();
            }
        }

        static void consumer1()
        {
            while (true)
            {
                // 如果没有产品了的话，就没办法继续消费
                fillCount.WaitOne();
                // 移除一个物品，并且获取缓冲区权限
                bufferMutex.WaitOne();
                var item = removeItemFromBuffer();
                // 释放缓冲区权限，并且消费完了，空的位置加一
                bufferMutex.ReleaseMutex();
                emptyCount.Release();
                ConsumItem(item);
            }
        }
        static void Main(string[] args)
        {
            int i, j;
            Console.WriteLine("请输入消费者个数：");
            i= Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("请输入生产者个数：");
            j = Convert.ToInt32(Console.ReadLine());

            for (int m = 0; m < j; m++)
            {
                producerThread = new Thread(producer1);
            }
            for (int n = 0; n < i; n++)
            {
                consumerThread = new Thread(consumer1);
            }
            producerThread.Start();
            consumerThread.Start();

        }
    }
}
