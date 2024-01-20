namespace Base.Entity;

// We have collected the features that all entities want to have by default in the Base class.
public abstract class BaseEntityWithId : BaseEntity
{
    public int Id { get; set; }
}
public abstract class BaseEntity  
{
    public int InsertUserId { get; set; }
    public DateTime InsertDate { get; set; }
    public int? UpdateUserId { get; set; }
    public DateTime? UpdateDate { get; set; }
    public bool? IsActive { get; set; }
}