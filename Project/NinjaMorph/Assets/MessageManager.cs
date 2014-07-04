using UnityEngine;
using System.Collections;

public class MessageManager : MonoBehaviour
{
	public GUIText messageObject;
	private Queue messageQueue;

	/// <summary>
	/// Clears the queue.
	/// </summary>
	public void clearQueue()
	{
		// Iterate through the queue
		while(messageQueue.Count > 0)
		{
			// Dequeue the GUI Text
			GUIText obj = (GUIText)messageQueue.Dequeue();
			// Destroy the object
			Destroy(obj);
		}
	}

	/// <summary>
	/// Queues the message.
	/// </summary>
	/// <param name="message">Message.</param>
	public void queueMessage(string message)
	{
		// Make sure we have a message
		if (message != null)
		{
			// Instantiate the new message
			GUIText newMessage = (GUIText)Instantiate(messageObject);
			newMessage.text = message;
			// Clone the current queue
			Queue oldQueue = (Queue)messageQueue.Clone();
			// Clear the current queue
			messageQueue.Clear();
			// Iterate through the queue and move them down
			while(oldQueue.Count > 0)
			{
				// Dequeue the GUI Text
				GUIText msg = (GUIText)oldQueue.Dequeue();
				// Check for null
				if (msg != null)
				{
					// Decrease the Y position
					msg.transform.position = new Vector3(msg.transform.position.x, msg.transform.position.y - 0.035f, msg.transform.position.z);
					// Add the message back to the message queue
					messageQueue.Enqueue(msg);
				}
			}
			// Queue the new message
			messageQueue.Enqueue(newMessage);
		}
	}

	// Use this for initialization
	void Start ()
	{
		// Initialize the queue
		messageQueue = new Queue ();
	}
}
