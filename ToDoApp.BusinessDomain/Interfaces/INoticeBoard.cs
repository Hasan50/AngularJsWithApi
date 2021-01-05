using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.BusinessDomain.Models;

namespace ToDoApp.BusinessDomain.Interfaces
{
    public interface INoticeBoard
    {
        List<NoticeBoard> GetNoticeBoard();
        
        NoticeBoard GetNoticeBoardById(string noticeId);
        NoticeDepartmentVIewModel CreateNoticeBoard(NoticeDepartmentVIewModel model);
    }
}
