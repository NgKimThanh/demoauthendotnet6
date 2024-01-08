using AutoMapper;
using DotNet6Authen.DTO;
using DotNet6Authen.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotNet6Authen.Service.GroupOfPorducts
{
    public class GroupOfProductService : IGroupOfProductService
    {

        private readonly DemoAuthenContext _demoAuthencontext;
        private readonly IMapper _mapper;

        public GroupOfProductService(DemoAuthenContext demoAuthencontext, IMapper mapper)
        {
            _demoAuthencontext = demoAuthencontext;
            _mapper = mapper;
        }

        public async Task GroupOfProduct_Add(GroupOfProductDTO item)
        {
            var book = _mapper.Map<GroupOfProduct>(item);
            _demoAuthencontext.GroupOfProduct!.Add(book);
            await _demoAuthencontext.SaveChangesAsync();
        }

        public async Task GroupOfProduct_Delete(int id)
        {
            var gop = await _demoAuthencontext.GroupOfProduct!.FirstOrDefaultAsync(c => c.ID == id);
            if (gop != null)
            {
                _demoAuthencontext.GroupOfProduct!.Remove(gop);
                await _demoAuthencontext.SaveChangesAsync();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<List<GroupOfProductDTO>> GroupOfProduct_GetAll()
        {
            var gops = await _demoAuthencontext.GroupOfProduct!.ToListAsync();
            return _mapper.Map<List<GroupOfProductDTO>>(gops);
        }

        public async Task<GroupOfProductDTO> GroupOfProduct_Get(int id)
        {
            var gop = await _demoAuthencontext.GroupOfProduct!.FirstOrDefaultAsync(c => c.ID == id);
            return _mapper.Map<GroupOfProductDTO>(gop);
        }

        public async Task GroupOfProduct_Update(int id, GroupOfProductDTO item)
        {
            var gop = await _demoAuthencontext.GroupOfProduct!.FirstOrDefaultAsync(c => c.ID == id);
            if (gop != null)
            {
                gop.Code = item.Code;
                gop.GroupProductName = item.GroupProductName;
                await _demoAuthencontext.SaveChangesAsync();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
