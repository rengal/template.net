using System;

namespace Resto.Data
{
    public partial class SurveyItemResponse
    {
        public SurveyItemResponse(SurveyItem surveyItem, bool mark, Guid deliveryOrderId)
        {
            this.surveyItem = surveyItem;
            this.mark = mark ? 100 : 0;
            this.deliveryOrderId = deliveryOrderId;
        }

        public bool Like
        {
            get { return mark == 100; }
        }
    }
}
