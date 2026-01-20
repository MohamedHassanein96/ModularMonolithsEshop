namespace Shared.Messaging.Dtos;

public record PaymentMessage(string CardName, string CardNumber, string Expiration, string Cvv, int PaymentMethod);
