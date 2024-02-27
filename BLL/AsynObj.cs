using System;
using System.Collections.Concurrent;
using System.Threading;
using BLL;
using Framework;

namespace MODEL
{
    public class AsynObj
    {
        public Object lockObj;

        public object LockObj
        {
            get => lockObj;
            set => lockObj = value;
        }

        private BlockingQueue<string> queue;
        private IAnalysis ana;
        private bool ifCheckHis;
        private CountdownEvent countdownEvent;

        public CountdownEvent CountdownEvent
        {
            get => countdownEvent;
            set => countdownEvent = value;
        }

        private ConcurrentBag<His> resultList;
        private ConcurrentBag<His> smallerList;
        private ConcurrentBag<His> invalidList;
        private ConcurrentBag<His> hisInvalidList;

        public BlockingQueue<string> Queue
        {
            get => queue;
            set => queue = value;
        }

        public IAnalysis Ana
        {
            get => ana;
            set => ana = value;
        }

        public bool IfCheckHis
        {
            get => ifCheckHis;
            set => ifCheckHis = value;
        }

        public ConcurrentBag<His> ResultList
        {
            get => resultList;
            set => resultList = value;
        }

        public ConcurrentBag<His> SmallerList
        {
            get => smallerList;
            set => smallerList = value;
        }

        public ConcurrentBag<His> InvalidList
        {
            get => invalidList;
            set => invalidList = value;
        }

        public ConcurrentBag<His> HisInvalidList
        {
            get => hisInvalidList;
            set => hisInvalidList = value;
        }
    }
}