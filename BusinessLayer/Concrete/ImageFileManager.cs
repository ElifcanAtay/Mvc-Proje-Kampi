using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class ImageFileManager : IImageFileService
    {
        IImageFileDal _ımageFileDal;

        public ImageFileManager(IImageFileDal ımageFileDal)
        {
            _ımageFileDal = ımageFileDal;
        }

        public ImageFile GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<ImageFile> GetList()
        {
            return _ımageFileDal.List();
        }

        public void ImageFileAdd(ImageFile ımageFile)
        {
            throw new NotImplementedException();
        }

        public void ImageFileDelete(ImageFile ımageFile)
        {
            throw new NotImplementedException();
        }

        public void ImageFileUpdate(ImageFile ımageFile)
        {
            throw new NotImplementedException();
        }
    }
}
