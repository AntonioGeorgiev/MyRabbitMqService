using System;
using Confluent.Kafka;
using MessagePack;
using MyRabbitMqService.Models;

namespace Producer
{
        internal class MsgPackSerializer<T> : ISerializer<T>
        {
        public byte[] Serialize(T data, SerializationContext context)
        {
            return MessagePackSerializer.Serialize<T>(data)
            ;
        }
        }
    }
