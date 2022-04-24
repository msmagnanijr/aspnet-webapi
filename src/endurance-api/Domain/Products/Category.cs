namespace Endurance.Domain.Products;
public class Category : Entity
{
    public Category(string name, string createdBy, string updatedBy)
    {

        Name = name;
        Active = true;
        CreatedBy = createdBy;
        UpdatedBy = updatedBy;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;

        Validate();

    }
    public string Name { get; private set; }
    public bool Active { get; private set; }

    public void Validate()
    {
        var contract = new Contract<Category>()
            .IsNotNullOrEmpty(Name, "Name")
            .IsGreaterOrEqualsThan(Name.Length, 3, "Name", "The name must have at least 3 characters")
            .IsNotNullOrEmpty(CreatedBy, "CreatedBy")
            .IsNotNullOrEmpty(UpdatedBy, "UpdatedBy");
        AddNotifications(contract);
    }
    public void Update(string name, bool active, string updatedBy)
    {
        Name = name;
        Active = active;
        UpdatedBy = updatedBy;
        UpdatedAt = DateTime.Now;
        Validate();
    }
}