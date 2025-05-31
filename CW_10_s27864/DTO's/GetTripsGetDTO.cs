using CW_10_s27864.Models;

namespace CW_10_s27864.DTO_s;

public class GetTripsGetDTO
{
    public int IdTrip { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime DateFrom { get; set; }

    public DateTime DateTo { get; set; }

    public int MaxPeople { get; set; }

    public virtual ICollection<GetCountryDTO> Country { get; set; } = new List<GetCountryDTO>();

    public virtual ICollection<GetClientDTO> Client { get; set; } = new List<GetClientDTO>(); 
}