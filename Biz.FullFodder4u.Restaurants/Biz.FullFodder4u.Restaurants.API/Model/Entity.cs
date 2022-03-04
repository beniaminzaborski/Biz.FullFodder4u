namespace Biz.FullFodder4u.Restaurants.API.Model;

public abstract class Entity
{
    protected Entity()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        Entity other = obj as Entity;
        if (other != null)
            return Id.Equals(other.Id);
        return false;
    }
}
