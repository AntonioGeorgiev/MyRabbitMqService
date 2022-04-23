using Confluent.Kafka;
using MessagePack;
using MyRabbitMqService.Models;
using System;

namespace ConsoleApp3
{
    internal class MsgPackDeserializer<T> : IDeserializer<T>
    {
        T IDeserializer<T>.Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return MessagePackSerializer.Deserialize<T>(data.ToArray());
        }

    };
}
