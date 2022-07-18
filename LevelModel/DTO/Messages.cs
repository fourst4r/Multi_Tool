using System;
using System.Collections;
using System.Collections.Generic;

using static LevelModel.DTO.Message;

namespace LevelModel.DTO
{
    public class Messages : IEnumerable<Message>
    {


        private List<Message> _messages;


        public Messages() {
            _messages = new List<Message>();
        }


        public void Add(string message, MessageType type = MessageType.Warning) {
            var m = new Message(message, type);

            if (MessageExist(m) == false)
                _messages.Add(m);
        }

        public void Add(Messages message) {
            if(message == null)
                return;

            foreach(var msg in message)
                _messages.Add(msg);
        }

        private bool MessageExist(Message message) {
            foreach (var msg in _messages) {
                if (msg.Content.Equals(message.Content, StringComparison.InvariantCultureIgnoreCase) && msg.Type == message.Type)
                    return true;
            }

            return false;
        }


        public IEnumerator<Message> GetEnumerator() {
            return _messages.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }


    }

}
