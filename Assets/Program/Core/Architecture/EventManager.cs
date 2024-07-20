using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// 事件系统轻便版，只通过一个枚举来触发，无额外信息
/// </summary>
namespace Ueels 
{

	/// <summary>
	/// framework of a goup of message infomation
	/// </summary>
	/*
	public class Event<E> where E: Enum
	{
		public E type;
		//other info:

		public Event(E type)
		{
			this.type = type;
		}
	}
	*/

	/// <summary>
	/// Subscriber need to response a info
	/// </summary>
	public partial class EventManager<E> where E: Enum
	{
		private  Dictionary<E, HashSet<Action>> topicResponsorTable;
		private  Queue<E> topics;//message queue

		public EventManager()
		{
			topics=new Queue<E>();
			
			FieldInfo[] fields = typeof(E).GetFields();
			topicResponsorTable=new Dictionary<E, HashSet<Action>>(fields.Length);
			for(int i = 1; i < fields.Length; i++)//fields[0] 不是枚举的字段，感兴趣的话可以自己测试一下
			{
				topicResponsorTable.Add((E)fields[i].GetValue(null),new HashSet<Action>());
				//Debug.log(fields[i] + " : " + (int)fields[i].GetValue(null));
			}
			
		}

		//to collect a news just happened
		/// <summary>
		/// 采集新闻,是记者上交新闻的渠道
		/// </summary>
		/// <param name="eventHappened"></param>
		public  void GatherEvent(E eventHappened)
		{
			topics.Enqueue(eventHappened);
			Notify();//即刻通知版本，也可选择外部控制通知时机
		}


		//for subscriber to decide what event are interesting for them
		public  void Subscribe(E news, Action subscriber)
		{
			topicResponsorTable[news].Add(subscriber);
		}
		public  void UnSubscribe(E news, Action subscriber)
		{
			topicResponsorTable[news].Remove(subscriber);
		}

		/// <summary>
		/// to notify topics to subscribers, and clear message queue
		/// 此框架实现的策略是一次简单的通知所有订阅者，并清空消息队列，可以自行更改此方法
		/// </summary>
		public  void Notify()
		{
			while (topics.Count>0)
			{
				E news = topics.Dequeue();
				foreach (var responsor in topicResponsorTable[news])
				{
					responsor();
				}
			}
		}

	}

}



