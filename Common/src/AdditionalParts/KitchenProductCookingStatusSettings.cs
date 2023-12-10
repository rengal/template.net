namespace Resto.Data
{
    public partial class KitchenProductCookingStatusSettings
    {
        public KitchenProductCookingStatusSettings(KitchenProductCookingStatus kitchenProductCookingStatus, string name, bool isActive) :
            this(kitchenProductCookingStatus, name)
        {
            this.isActive = isActive;
        }

        public KitchenProductCookingStatusSettings(KitchenProductCookingStatus kitchenProductCookingStatus, string name, bool isActive, int autoCompleteInterval) :
            this(kitchenProductCookingStatus, name, isActive)
        {
            this.autoCompleteInterval = autoCompleteInterval;
        }

        public KitchenProductCookingStatusSettings(KitchenProductCookingStatus kitchenProductCookingStatus, string name, bool isActive, int autoCompleteInterval, bool needPrintServiceCheque) :
            this(kitchenProductCookingStatus, name, isActive, autoCompleteInterval)
        {
            this.needPrintServiceCheque = needPrintServiceCheque;
        }
    }
}