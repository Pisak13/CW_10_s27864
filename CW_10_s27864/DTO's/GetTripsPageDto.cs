namespace CW_10_s27864.DTO_s;

public class GetTripsPageDto
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public List<GetTripsGetDTO> Trips { get; set; } = [];
}