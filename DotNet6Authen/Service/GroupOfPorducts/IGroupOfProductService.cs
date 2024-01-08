using DotNet6Authen.DTO;

namespace DotNet6Authen.Service.GroupOfPorducts
{
    public interface IGroupOfProductService
    {
        Task GroupOfProduct_Add(GroupOfProductDTO item);

        Task GroupOfProduct_Delete(int id);

        Task<List<GroupOfProductDTO>> GroupOfProduct_GetAll();

        Task<GroupOfProductDTO> GroupOfProduct_Get(int id);

        Task GroupOfProduct_Update(int id, GroupOfProductDTO item);
    }
}
