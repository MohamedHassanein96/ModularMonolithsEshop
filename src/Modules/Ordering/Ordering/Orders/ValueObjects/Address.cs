namespace Ordering.Orders.ValueObjects;

public record Address
{
    public string FirstName { get; } = default!;
    public string LastName { get; } = default!;
    public string? EmailAddress { get; } = default!;
    public string AddressLine { get; } = default!;
    public string Country { get; } = default!;
    public string State { get; } = default!;
    public string ZipCode { get; } = default!;

    // Because it's (immutable- value object) get only properties we have to enable EF from creating A new object throuth providing him a CTOR the he will use the 
    // reflection to fill the get only properties
    // unlike Entity is mutable (pivate set ) so EF can create an object withouth providing him with CTOR 
    protected Address()
    {

    }

    // functionallty of Ctor is assigining fields -Single Responsibility Principle
    private Address(string firstName, string lastName, string emailAddress, string addressLine, string country, string state, string zipCode)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        AddressLine = addressLine;
        Country = country;
        State = state;
        ZipCode = zipCode;
    }
    // Validation and domain invariants to ensure correct Address creation
    public static Address Of(string firstName, string lastName, string emailAddress, string addressLine, string country, string state, string zipCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(emailAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(addressLine);

        return new Address(firstName, lastName, emailAddress, addressLine, country, state, zipCode);
    }

}
