using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using Nigel.Core.Helper;

namespace Nigel.Core.Tools
{
    /// <summary>
    /// 流量控制
    /// </summary>
    public class FlowControl : TimerBase
    {
        private static object syncLock = new object();
        private static FlowControl flow = null;
        private decimal _setSize = 128 * 8;
        private decimal _netBytes = 0;
        private decimal _Kbyte = 1024;
        private Action<decimal> _actionMonitoring = null;
        private int _interval = 1000;

        public FlowControl(int interval = 1000)
            : base(interval)
        {
            this._interval = interval;
            this.timer.AutoReset = true;
            this.timer.Start();
        }
        /// <summary>
        /// 触发周期
        /// <remarks>默认1000毫秒一次</remarks>
        /// </summary>
        public int Interval
        {
            get { return _interval; }
            set
            {
                lock (syncLock)
                {
                    _interval = value;
                    this.timer.Interval = value;
                }
            }
        }

        /// <summary>
        /// 每秒写入大小
        /// <remarks>单位KB</remarks>
        /// </summary>
        public decimal Size
        {
            get { return _setSize / 8; }
            set
            {
                lock (syncLock)
                    _setSize = value * 8;
            }
        }

        public static FlowControl Singleton
        {
            get
            {
                if (flow == null)
                {
                    lock (syncLock)
                    {
                        if (flow == null)
                            flow = new FlowControl();
                    }
                }
                return flow;
            }
        }

        public override void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_actionMonitoring != null)
                _actionMonitoring.Invoke(this._netBytes / 8);
        }

        /// <summary>
        /// 监控每秒流量
        /// </summary>
        /// <param name="action"></param>
        public void Monitoring(Action<decimal> action)
        {
            _actionMonitoring = action;
        }

        /// <summary>
        /// 流量控制
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="callback"></param>
        public void Limit(DataTable dataTable, Action<DataRow[]> callback)
        {
            Monitor.Enter(syncLock);
            try
            {
                if (dataTable == null || callback == null)
                    return;

                decimal _mSize = _setSize;

                //复制表结构
                DataTable newDataTable = dataTable.Clone();

                List<DataRow> rows = new List<DataRow>();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                Array.ForEach<DataRow>(dataTable.Select(), (dr) =>
                {
                    //清除计算表
                    newDataTable.Clear();
                    //复制新的DataRow行进行计算
                    newDataTable.ImportRow(dr);

                    rows.Add(dr);
                    try
                    {
                        //获取单条记录byte
                        byte[] newDataTableByte = newDataTable.ToBinary();
                        decimal drKB = newDataTableByte.Length / _Kbyte;
                        //单位限制的剩余流量
                        _mSize -= drKB;
                        //总流量
                        _netBytes += drKB;
                    }
                    catch { }

                    //如果流量超过了限制数
                    if (_mSize < 0)
                    {
                        //时间再一秒内 则休眠
                        if (stopwatch.Elapsed.Milliseconds < _interval)
                            Thread.Sleep(_interval - stopwatch.Elapsed.Milliseconds);

                        callback.Invoke(rows.ToArray());

                        rows.Clear();
                        //重启计时器  
                        stopwatch.Reset();
                        stopwatch.Start();
                        //初始化限制
                        _mSize = _setSize;
                        _netBytes = 0;
                    }
                });
                //时间再一秒内 则休眠
                if (stopwatch.Elapsed.Milliseconds < _interval)
                    Thread.Sleep(_interval - stopwatch.Elapsed.Milliseconds);
                callback.Invoke(rows.ToArray());
                _netBytes = 0;
            }
            finally
            {
                Monitor.Exit(syncLock);
            }
        }

        /// <summary>
        /// 流量控制
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="callback"></param>
        public void Limit<TEntity>(List<TEntity> entities, Action<TEntity[]> callback)
            where TEntity : class, new()
        {
            if (entities == null || callback == null || entities.Count == 0)
                return;

            Limit<TEntity>(entities.ToArray(), callback);
        }

        /// <summary>
        /// 流量控制
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="callback"></param>
        public void Limit<TEntity>(TEntity[] entities, Action<TEntity[]> callback)
            where TEntity : class, new()
        {
            Monitor.Enter(syncLock);
            try
            {
                if (entities == null || callback == null || entities.Length == 0)
                    return;

                decimal _mSize = _setSize;

                //复制表结构
                List<TEntity> newEntities = new List<TEntity>();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                Array.ForEach<TEntity>(entities.ToArray(), (entity) =>
                {
                    newEntities.Add(entity);
                    try
                    {
                        //获取单条记录byte
                        byte[] newEntityByte = entity.ToBinary();
                        decimal kb = newEntityByte.Length / _Kbyte;
                        //单位限制的剩余流量
                        _mSize -= kb;
                        //总流量
                        _netBytes += kb;
                    }
                    catch { }

                    //如果流量超过了限制数
                    if (_mSize < 0)
                    {
                        //时间再一秒内 则休眠
                        if (stopwatch.Elapsed.Milliseconds < _interval)
                            Thread.Sleep(_interval - stopwatch.Elapsed.Milliseconds);

                        callback.Invoke(newEntities.ToArray());

                        newEntities.Clear();
                        //重启计时器  
                        stopwatch.Reset();
                        stopwatch.Start();
                        //初始化限制
                        _mSize = _setSize;
                        _netBytes = 0;
                    }
                });
                //时间再一秒内 则休眠
                if (stopwatch.Elapsed.Milliseconds < _interval)
                    Thread.Sleep(_interval - stopwatch.Elapsed.Milliseconds);
                callback.Invoke(newEntities.ToArray());
                _netBytes = 0;
            }
            finally
            {
                Monitor.Exit(syncLock);
            }
        }
    }
}
