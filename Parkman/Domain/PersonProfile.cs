using System;

namespace Parkman.Domain;

public class PersonProfile
{
    public string UserId { get; private set; } = null!;
    public ApplicationUser User { get; private set; } = null!;

    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public DateOnly DateOfBirth { get; private set; }

    private PersonProfile() { }

    public PersonProfile(string firstName, string lastName, DateOnly dateOfBirth)
    {
        Update(firstName, lastName, dateOfBirth);
    }

    public void Update(string firstName, string lastName, DateOnly dateOfBirth)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required", nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }

    internal void SetUser(ApplicationUser user)
    {
        User = user;
        UserId = user.Id;
    }
}
