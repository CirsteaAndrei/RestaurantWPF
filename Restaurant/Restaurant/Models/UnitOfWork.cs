//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Restaurant.Models
//{
//    public class UnitOfWork
//    {
//        public CarsRepository Cars { get; }
//        public CategoriesRepository Categories { get; }
//        //public ClassRepository Classes { get; }

//        //public GradesRepository Grades { get; }

//        private readonly AppDbContext _dbContext;

//        public UnitOfWork
//        (
//            AppDbContext dbContext,
//            CarsRepository carsRepository,
//            CategoriesRepository categoriesRepository
//        )
//        {
//            _dbContext = dbContext;
//            Cars = carsRepository;
//            Categories = categoriesRepository;
//        }

//        public void SaveChanges()
//        {
//            try
//            {
//                _dbContext.SaveChanges();
//            }
//            catch (Exception exception)
//            {
//                var errorMessage = "Error when saving to the database: "
//                    + $"{exception.Message}\n\n"
//                    + $"{exception.InnerException}\n\n"
//                    + $"{exception.StackTrace}\n\n";

//                Console.WriteLine(errorMessage);
//            }
//        }
//    }
//}
