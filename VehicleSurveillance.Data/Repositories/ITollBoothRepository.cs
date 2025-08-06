using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Data.Models;

namespace VisualSoft.Surveillance.Payment.Data.Repositories
{
    public interface ITollBoothRepository
    {
        Task<IEnumerable<TollBoothDataModel>?> GetAll();
        Task<TollBoothDataModel> Create(TollBoothDataModel tollBooth);
        Task<TollBoothDataModel> Update(TollBoothDataModel tollBooth);
        Task<TollBoothDataModel> GetById(Guid id);
        Task<bool> Delete(Guid id);
        Task<(Guid? EnterBoothId, Guid? ExitBoothId)> GetBoothIds(string enterBooth, string exitBooth);
        Task<string> GetBoothNameById(Guid boothId);
    }
}
