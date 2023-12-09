<?xml version="1.0" encoding="utf-8"?>
<x:stylesheet version="1.0" xmlns:x="http://www.w3.org/1999/XSL/Transform">
  <x:output method="xml" version="1.0" encoding="utf-8" indent="yes" />

  <x:variable name="Res" select="/*/Settings/Resources" />
  <!-- ChequeLocalResources (strings as attrs) -->

  <!-- Чек: ошибка, неподдерживаемый корень XML -->
  <x:template match="/*">
    <doc>
      <f2>ОШИБКА</f2>
      <f1>
        Неподдерживаемый тип чеков: <x:value-of select="name(/*)" />
      </f1>
    </doc>
  </x:template>

  <!-- Чек: квитанция об оплате / квитанция о сторнировании -->
  <x:template match="/Receipt">
    <doc>
      <x:call-template name="CommonHeader" />
      <x:copy-of select="BeforeCheque/*" />
      <x:call-template name="CashRegisterHeader" />
      <center>
        <x:choose>
          <x:when test="@IsStorno = 'false'">
            <x:value-of select="$Res/@OrderPaymentReceipt" />
          </x:when>
          <x:otherwise>
            <x:value-of select="$Res/@StornoCheque" />
          </x:otherwise>
        </x:choose>
      </center>
      <x:call-template name="TimeCashierOrderHeader" />
      <x:call-template name="OrderInfoHeader" />
      <x:apply-templates select="Sales" />
      <x:if test="@ResultSum != @Sum">
        <pair left="{$Res/@BillFooterTotalPlain}" right="{iiko:FormatMoney(@Sum)}" />
      </x:if>
      <x:apply-templates select="Discounts" />
      <x:apply-templates select="Increases" />
      <x:if test="/*/@VatSum != 0">
        <pair left="{$Res/@ResultSumWithoutNds}" right="{iiko:FormatMoney(@ResultSum - @VatSum)}" />
        <x:for-each select="Vats/ReceiptVat">
          <pair left="{iiko:Format($Res/@VatFormat, iiko:FormatPercent(@VatPercent))}" right="{iiko:FormatMoney(@VatSum)}" />
        </x:for-each>
        <x:if test="count(Vats/ReceiptVat) > 1">
          <pair left="{$Res/@VatSum}" right="{iiko:FormatMoney(/*/@VatSum)}" />
        </x:if>
      </x:if>
      <pair left="{$Res/@BillFooterTotal}" right="{iiko:FormatMoney(@ResultSum)}" />
      <x:apply-templates select="Payments" />
      <x:call-template name="CommonFooter" />
      <x:if test="@IsStorno = 'false'">
        <np />
        <line />
        <center>
          <x:value-of select="$Res/@Signature" />
        </center>
      </x:if>
      <x:copy-of select="AfterCheque/*" />
    </doc>
  </x:template>

  <!-- Чек: Новый чек квитанция об оплате / квитанция о сторнировании, с номерами квитанций -->
  <x:template match="ReceiptSplitByChequeNumber">
    <doc>
      <x:for-each select="Payments/Payment">
        <x:if test="@Sum > 0">
          <x:call-template name="CommonHeader" />
          <x:for-each select="/*/BeforeCheque/Value">
            <left>
              <x:value-of select="." />
            </left>
          </x:for-each>
          <x:call-template name="CashRegisterHeader" />
          <x:call-template name="ChequeNumberHeader" />
          <center>
            <x:choose>
              <x:when test="/*/@IsStorno = 'false'">
                <x:value-of select="$Res/@OrderPaymentReceipt" />
              </x:when>
              <x:otherwise>
                <x:value-of select="$Res/@StornoCheque" />
              </x:otherwise>
            </x:choose>
          </center>
          <x:call-template name="TimeCashierOrderHeader" />
          <x:call-template name="OrderInfoHeader" />
          <x:apply-templates select="/*/Sales" />
          <x:if test="/*/@ResultSum != /*/@Sum">
            <pair left="{$Res/@BillFooterTotalPlain}" right="{iiko:FormatMoney(/*/@Sum)}" />
          </x:if>
          <x:apply-templates select="/*/Discounts" />
          <x:apply-templates select="/*/Increases" />
          <x:if test="/*/@VatSum != 0">
            <pair left="{$Res/@ResultSumWithoutNds}" right="{iiko:FormatMoney(@ResultSum - @VatSum)}" />
            <x:for-each select="Vats/ReceiptVat">
              <pair left="{iiko:Format($Res/@VatFormat, iiko:FormatPercent(@VatPercent))}" right="{iiko:FormatMoney(@VatSum)}" />
            </x:for-each>
            <x:if test="count(Vats/ReceiptVat) > 1">
              <pair left="{$Res/@VatSum}" right="{iiko:FormatMoney(/*/@VatSum)}" />
            </x:if>
          </x:if>
          <pair left="{$Res/@BillFooterTotal}" right="{iiko:FormatMoney(/*/@ResultSum)}" />
          <x:apply-templates select="." />
          <x:call-template name="CommonFooter" />
          <x:if test="/*/@IsStorno = 'false'">
            <np />
            <line />
            <center>
              <x:value-of select="$Res/@Signature" />
            </center>
          </x:if>
          <x:for-each select="/*/AfterCheque/Value">
            <left>
              <x:value-of select="." />
            </left>
          </x:for-each>
        </x:if>
        <x:if test="position() != last()" >
          <pagecut/>
        </x:if>
      </x:for-each>
    </doc>
  </x:template>

  <!-- Чек: внесение / изъятие -->
  <x:template match="/PayInOut">
    <doc>
      <x:call-template name="CommonHeader" />
      <center>
        <x:choose>
          <x:when test="@IsPayIn = 'true'">
            <x:value-of select="$Res/@PayIn" />
          </x:when>
          <x:when test="@IsPayIn = 'false'">
            <x:value-of select="$Res/@PayOut" />
          </x:when>
        </x:choose>
      </center>
      <x:choose>
        <x:when test="@IsChangePayIn = 'true'">
          <pair left="{$Res/@PayInOutType}" right="{$Res/@ChangePayIn}" />
        </x:when>
        <x:when test="@EventTypeName != ''">
          <pair left="{$Res/@PayInOutType}" right="{@EventTypeName}" />
        </x:when>
      </x:choose>
      <x:if test="@AccountName != ''">
        <pair left="{$Res/@Account}" right="{@AccountName}" />
      </x:if>
      <pair left="{$Res/@CurrentDate}" right="{iiko:FormatLongDateTime(Info/@OperationTime)}" />
      <pair left="{$Res/@BillHeaderCashier}"  right="{Info/@CashierName}" />
      <line />
      <x:choose>
        <x:when test="@IsPayIn = 'true'">
          <pair left="{$Res/@PaidIn}" right="{iiko:FormatMoney(@Sum)}" />
        </x:when>
        <x:when test="@IsPayIn = 'false'">
          <pair left="{$Res/@PaidOut}" right="{iiko:FormatMoney(@Sum)}" />
        </x:when>
      </x:choose>
      <x:if test="@Comment != ''">
        <x:value-of select="iiko:Format($Res/@ReasonFormat, @Comment)" />
      </x:if>
      <x:call-template name="CommonFooter" />
    </doc>
  </x:template>

  <!-- Чек: предоплата / возврат предоплаты -->
  <x:template match="/Prepay">
    <doc>
      <center>
        <x:choose>
          <x:when test="@IsReturn = 'true'">
            <x:value-of select="$Res/@PrepayReturnCheque" />
          </x:when>
          <x:when test="@IsReturn = 'false'">
            <x:value-of select="$Res/@PrepayCheque" />
          </x:when>
        </x:choose>
      </center>
      <x:call-template name="TimeCashierOrderHeader" />
      <left>
        <x:value-of select="iiko:Format($Res/@BillHeaderWaiterPattern, @WaiterName)" />
      </left>
      <line />
      <pair left="{@PaymentTypeName}" right="{iiko:FormatMoney(@Sum)}" />
      <x:call-template name="CommonFooter" />
    </doc>
  </x:template>

  <!-- Sales list -->
  <x:template match="Sales">
    <line />
    <x:choose>
      <x:when test="count(Sale) = 0">
        <x:value-of select="$Res/@ZeroChequeBody" />
        <line />
      </x:when>
      <x:when test="/*/@IsFullCheque = 'true'">
        <table>
          <columns>
            <column formatter="split" />
            <column align="right" autowidth="" />
            <column align="right" autowidth="" />
          </columns>
          <cells>
            <x:if test="/*/Info/@IsForReport = 'false'">
              <ct>
                <x:value-of select="$Res/@NameTitle" />
              </ct>
              <ct>
                <x:value-of select="$Res/@AmountWithPriceTitle" />
              </ct>
              <ct>
                <x:value-of select="$Res/@ProductSum" />
              </ct>
              <linecell />
            </x:if>
            <x:apply-templates select="Sale" />
          </cells>
        </table>
        <line />
      </x:when>
    </x:choose>
  </x:template>

  <x:template match="Sale">
    <c>
      <x:value-of select="@Name" />
    </c>
    <ct>
      <x:value-of select="concat(iiko:FormatAmount(@Amount), 'x', iiko:FormatMoneyMin(@Price))" />
    </ct>
    <ct>
      <x:value-of select="iiko:FormatMoney(@Sum)" />
    </ct>
    <x:if test="@Increase != '0'">
      <c>
        <x:value-of select="$Res/@Increase" />
      </c>
      <ct>
        <x:value-of select="iiko:FormatPercent(@Increase)" />
      </ct>
      <ct>
        <x:value-of select="iiko:FormatMoney(@IncreaseSum)" />
      </ct>
    </x:if>
    <x:if test="@Discount != '0'">
      <c>
        <x:value-of select="$Res/@Discount" />
      </c>
      <ct>
        <x:value-of select="iiko:FormatPercent(@Discount)" />
      </ct>
      <ct>
        <x:value-of select="iiko:FormatMoney(@DiscountSum)" />
      </ct>
    </x:if>
  </x:template>

  <!-- Payments list -->
  <x:template match="Payments">
    <line />
    <x:apply-templates select="../Prepays/Prepay" />
    <x:apply-templates select="Payment" />
    <x:if test="/*/@HasMultiplePayments = 'true'">
      <pair left="{$Res/@Total}" right="{iiko:FormatMoney(/*/@PaymentsSum)}" />
    </x:if>
    <x:if test="number(/*/@ChangeSum) > 0">
      <pair left="{$Res/@Change}" right="{iiko:FormatMoney(/*/@ChangeSum)}" />
    </x:if>
  </x:template>

  <x:template match="Prepay">
    <pair left="{iiko:Format($Res/@PrepayTemplate, @Name)}" right="{iiko:FormatMoney(@Sum)}" />
  </x:template>

  <x:template match="Payment">
    <pair left="{@Name}" right="{iiko:FormatMoney(@Sum)}" />
    <x:if test="@Comment != ''">
      <left>
        <x:value-of select="@Comment" />
      </left>
    </x:if>
  </x:template>

  <!-- Discounts/increases list -->
  <x:template match="Discounts|Increases">
    <x:apply-templates select="DiscountIncrease" />
  </x:template>

  <x:template match="DiscountIncrease/SimpleCorrectionItem">
    <x:call-template name="SimpleCorrectionItemBase" />
  </x:template>

  <x:template match="DiscountIncrease/DiscountCardItem">
    <x:call-template name="SimpleCorrectionItemBase" />
    <x:if test="@CardNumber != ''">
      <x:value-of select="iiko:Format($Res/@CardPattern, @CardNumber)" />
    </x:if>
  </x:template>

  <x:template match="DiscountIncrease[not(contains(' SimpleCorrectionItem DiscountCardItem ', concat(' ', name(*), ' ')))]">
    <x:copy-of select="DefaultMarkup/*" />
  </x:template>

  <x:template name="SimpleCorrectionItemBase">
    <table>
      <columns>
        <column formatter="split" />
        <column />
        <column align="right" autowidth="" />
      </columns>
      <cells>
        <c>
          <x:value-of select="@Name" />
        </c>
        <c>
          <x:value-of select="iiko:FormatPercent(@Percent)" />
        </c>
        <ct>
          <x:value-of select="iiko:FormatMoney(@Price)" />
        </ct>
      </cells>
    </table>
  </x:template>

  <!-- Header -->
  <x:template name="CommonHeader">
    <x:if test="/*/Info/@IsForReport = 'false'">
      <left>
        <split>
          <x:value-of select="/*/Settings/@ChequeHeader" />
        </split>
      </left>
    </x:if>
  </x:template>

  <x:template name="CashRegisterHeader">
    <x:if test="/*/Info/@IsForReport = 'false'">
      <pair left="{iiko:Format($Res/@CashRegisterFormat, /*/Info/@CashRegisterNumber)}" right="{/*/Settings/@GroupName}" />
      <pair left="{$Res/@HeadCashRegisterShift}" right="{/*/Info/@CafeSessionNumber}" />
    </x:if>
  </x:template>

  <x:template name="ChequeNumberHeader">
    <x:if test="@IsFiscal = 'false'">
      <left>
        <x:value-of select="iiko:Format($Res/@ChequeNumberFormat, @ChequeNumber)" />
      </left>
    </x:if>
  </x:template>

  <x:template name="TimeCashierOrderHeader">
    <pair left="{$Res/@CurrentDate}" right="{iiko:FormatLongDateTime(/*/Info/@OperationTime)}" />
    <pair left="{iiko:Format($Res/@BillHeaderCashierPattern, /*/Info/@CashierName)}" right="{iiko:Format($Res/@BillHeaderOrderNumberPattern, /*/@OrderNumber)}" />
  </x:template>

  <x:template name="OrderInfoHeader">
    <left>
      <x:value-of select="iiko:Format($Res/@BillHeaderWaiterPattern, /*/@WaiterName)" />
    </left>
    <left>
      <split>
        <x:value-of select="iiko:Format($Res/@BillHeaderLocationAndGuestsPattern, /*/@SectionName, /*/@TableNumber, /*/@GuestsAmount)" />
      </split>
    </left>
  </x:template>

  <!-- Footer -->
  <x:template name="CommonFooter">
    <x:if test="/*/Info/@IsForReport = 'false'">
      <center>
        <x:value-of select="iiko:Format($Res/@AllSumsInFormat, /*/Settings/@CurrencyName)" />
      </center>
      <np />
      <center>
        <split>
          <x:value-of select="/*/Settings/@ChequeFooter" />
        </split>
      </center>
    </x:if>
  </x:template>

</x:stylesheet>
