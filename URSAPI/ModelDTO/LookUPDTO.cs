using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URSAPI.ModelDTO
{
    public class LookUPDTO
    {
    }

    public class AddNewCategory
    {
        public Int32 MainCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
    }
   

    public class LookUpItemDTO
    {
        public Int32 CategoryId { get; set; }
        public string Type { get; set; }
        public long Id { get; set; }
        public string Key { get; set; }
        public short DisplayOrder { get; set; }
        public string Value { get; set; }
    }

    public class MainCategoryDTO {
        public long Id { get; set; }
        public string MainCategoryName { get; set; }
        public long created_by { get; set; }
        public DateTime created_time { get; set; }
        public long last_updated_by { get; set; }
        public DateTime last_updated_time { get; set; }
    }

    public class CategoryDTO {
        public long Id { get; set; }
        public string Question { get; set; }
        public long MainCategoryId { get; set; }
        public long created_by { get; set; }
        public DateTime created_time { get; set; }
        public long last_updated_by { get; set; }
        public DateTime last_updated_time { get; set; }
    }

    public class SubCategoryDTO {
        public long Id { get; set; }
        public string Risk { get; set; }
        public long MainCategoryId { get; set; }
        public long CategoryId { get; set; }
        public string QuestionandExplanation { get; set; }
        public long created_by { get; set; }
        public DateTime created_time { get; set; }
        public long last_updated_by { get; set; }
        public DateTime last_updated_time { get; set; }
    }
    public class categorySaveDTO {
        public string categoryName { get; set; }
        public string question { get; set; }
        public string risk { get; set; }
        public string questiontoask { get; set; }
        public string explanation { get; set; }
        public long headernumber { get; set; }
        public string questionnumber { get; set; }
        public questionandexplanation JSONdata { get; set; }
    }

    public class questionandexplanation
    {
        public string question { get; set; }
        public string explanation { get; set; }
        public string risk { get; set; }
        public string questionnumber { get; set; }
    }

    public class JSONRiskexplanDTO
    {
        public questionandexplanation JSONdata { get; set; }
    }

}
