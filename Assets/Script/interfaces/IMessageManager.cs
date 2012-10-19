using UnityEngine;
using System.Collections;

/**
 * The IMessageManager should be implemented by objects intending to fulfill Entity to Entity synchronous messaging (also known as events or signals).
 * <p>The interface provides an observer pattern oriented manager allowing any Entity to listen to anything on any other Entity.</p>
 * <p>Note, the author is not a fan of observer pattern and provides this manager with a note of caution - there is <i>always</i> a better way to communicate than to fire shots into the dark!</p>
 * <p>This manager is intentionally abstract / generic.  It allows expressive synchronous events - i.e. use anything as a message (string, enumerator, type, state based object etc).</p>
 * <p>It may make more sense to handle events using an alternative, event or signal specific library.  Adapt one as an IEntity and inject into any scene as needed.</p>
 */
public interface IMessageManager : IResettable
{
	/**
	 * Register an entity's interest in a subject.
	 * @param	subscriber	Entity listening / observing for messages.
	 * @param	message	Specific message to listen for.
	 * @param	handler	Function to pass observed messages to: receives Message & Sender and returns true if send propogation is to continue (true should be default behavior).
	 * @param	?sender	Only listen to messages from this entity.
	 * @param	?senderClassType	Only listen to messages from this type of entity.
	 * @param	?isRemovedAfterFirstSend	Once a message has been received, no longer listen for further messages under the same criteria.
	 * @type	<M>	Messages can be any type.  For recursive types use Enums.
	 * @type	<T>	Senders' type.
	 */
	void AddSubscriber<M,T> (	IEntity subscriber,
								M message,
								IEntity sender,
								string handlerFuncName,
								bool isRemovedAfterFirstSend = false );
	/**
	 * Retrieve all entity's interested in a subject.
	 * <p>All parameters are optional to allow wildcard filtering.</p>
	 * @param	?subscriber	Entity listening / observing for messages.
	 * @param	?message	Specific message to listen for.
	 * @param	?handler	Function to pass observed messages to.
	 * @param	?sender	Only listen to messages from this entity.
	 * @param	?senderClassType	Only listen to messages from this type of entity.
	 * @return	An array of entities corresponding to the specified filters.
	 * @type	<M>	Messages can be any type.  For recursive types use Enums.
	 * @type	<T>	Senders' type.
	 */
	IEntity[] GetSubscribers<M,T> (IEntity subscriber,
										M message,
										string handlerFuncName,
										IEntity sender );
	/**
	 * Unsubscribes entities matching the specified criteria.
	 * @param	?subscriber	Entity listening / observing for messages.
	 * @param	?message	Specific message to listen for.
	 * @param	?handler	Function to pass observed messages to.
	 * @param	?sender	Only listen to messages from this entity.
	 * @param	?senderClassType	Only listen to messages from this type of entity.
	 * @type	<M>	Messages can be any type.  For recursive types use Enums.
	 * @type	<T>	Senders' type.
	 */
	void RemoveSubscribers<M,T> (	IEntity subscriber,
									M message,
									string handlerFuncName,
									IEntity sender );
	/**
	 * Dispatch a message from a specific entity.
	 * @param	message	Message to dispatch.
	 * @param	sender	The originator of the message (can be spoofed).
	 * @param	?isBubbleDown	Set to true if you want to dispatch this message to the sender's children.
	 * @param	?isBubbleUp	Set to true if you want to dispatch this message to the sender's parent.
	 * @param	?isBubbleEverywhere	Set to true if you want to dispatch this message to the entity traversal stack.
	 * @type	<M>	Messages can be any type.  For recursive types use Enums.
	 */
	void SendMessage<M> (	M message,
							IEntity sender,
							bool isBubbleDown = false,
							bool isBubbleUp = false,
							bool isBubbleEverywhere = false );
}