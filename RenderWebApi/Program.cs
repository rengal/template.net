using log4net.Appender;
using Resto.Common.Properties;
using Resto.Data;
using Resto.Framework.Common;
using Resto.Framework.Common.Currency;
using Resto.Framework.Data;
using Resto.Front.PrintTemplates.Cheques.Resources;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<ILogFactory, LogFactoryInternal>();
builder.Services.AddSingleton<IBaseEntityManager>(provider =>
{
    var suppressPersistentEntityCheck = false; // Set the value as needed
    return new Resto.Data.RMSEntityManager(suppressPersistentEntityCheck);
});

 EntityManager.EntitiesProviderFactory = new CommonEntitiesProviderFactory();

var currency = new Currency(ChequesPreviewLocalResources.CurrencyIsoName,
    ChequesPreviewLocalResources.CurrencyShortName, ChequesPreviewLocalResources.CurrencyShortName2,
    ChequesPreviewLocalResources.CurrencyShortName2);

var corporation = new Corporation(Guid.NewGuid(), string.Empty, ChequesPreviewLocalResources.RestaurantName, null,
    string.Empty, string.Empty, currency,
    DistributionAlgorithmType.DISTRIBUTION_NOT_SPECIFIED,
    VatAccounting.VAT_INCLUDED_IN_PRICE,
    new PersonalDataProcessingSettings(true, true, CustomerDataTransferType.SEND),
    new AddressShowTypeSettings(false, AddressShowType.LEGACY, false),
    new DateFormatSettings(WeekDays.MONDAY, 4, 1, 0, 15),
    AllowableDeviationAction.NOT_NOTIFY,
    new StoreAccountingSettings());

CurrencyHelper.SetCurrencyProviderResolver(() => corporation);
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
