<?xml version="1.0" encoding="utf-8"?>
<x:stylesheet version="1.0" xmlns:x="http://www.w3.org/1999/XSL/Transform">
  <x:output method="xml" version="1.0" encoding="utf-8" indent="yes" />

  <x:variable name="Res" select="/*/Settings/Resources" />
  <!-- ChequeLocalResources (strings as attrs) -->
  <x:variable name="CommonBillFooter" select="/*/BillFooter/@Value" />

  <!-- Чек: ошибка, неподдерживаемый корень XML -->
  <x:template match="/*">
    <doc>
      <f2>ОШИБКА</f2>
      <f1>
        Неподдерживаемый тип чеков: <x:value-of select="name(/*)" />
      </f1>
    </doc>
  </x:template>

  <x:template match="/DeliveryBill">
    <doc bell="">
      <x:apply-templates select="DeliveryInfo" mode="Header" />
      <x:apply-templates select="DeliveryOrderInfo" mode="Body">
        <x:with-param name="SplitBetweenPersons" select="DeliveryInfo/@SplitBetweenPersons"/>
        <x:with-param name="PersonsCount" select="DeliveryInfo/@PersonsCount"/>
      </x:apply-templates>
      <x:apply-templates select="DeliveryInfo" mode="Footer" />
    </doc>
  </x:template>

  <!-- Header -->
  <x:template match="DeliveryInfo" mode="Header">
    <left>
      <split>
        <x:value-of select="/*/Settings/@ChequeHeader" />
      </split>
    </left>
    <np />
    <center>
      <x:value-of select="$Res/@DeliveryBill" />
    </center>
    <center>
      <x:value-of select="iiko:Format($Res/@BillHeaderOrderNumberPattern, @DeliveryNumber)" />
    </center>
    <np />
    <left>
      <x:value-of select="$Res/@ConsignorColon" />
    </left>
    <left>
      <x:value-of select="@RestourantLegalName"/>
    </left>
    <left>
      <x:value-of select="iiko:Format($Res/@HeadCafeTaxIdNoColonFormat, @TaxId)" />
    </left>
    <np />
    <left>
      <x:value-of select="$Res/@ConsigneeColon" />
    </left>
    <left>
      <x:value-of select="iiko:Format($Res/@ClientFullNameFormat, @CustomerSurname, @CustomerNameAndPatronymic)" />
    </left>
    <left>
      <x:value-of select="@PhoneNumber" />
    </left>
    <np />
    <x:if test="@IsSelfService != 'true'">
      <left>
        <x:value-of select="iiko:Format($Res/@AddressFormat, @Address)" />
      </left>
      <left>
        <x:value-of select="@AdressComment" />
      </left>
      <x:if test="@Region != 'null' and @Region != ''">
        <left>
          <x:value-of select="iiko:Format($Res/@AddressRegionFormat, @Region)" />
        </left>
      </x:if>
    <np />        
    </x:if>
    <x:if test="DiscountCardNumber != 'null' and DiscountCardNumber != ''">
      <left>
        <x:value-of select="iiko:Format($Res/DiscountCardNumber, DiscountCardNumber)" />
      </left>
    </x:if>
    <table>
      <columns>
        <column autowidth="" />
        <column align="left" />
      </columns>
      <cells>
        <ct>
          <x:value-of select="$Res/@ManagerColon" />
        </ct>
        <ct>
          <x:value-of select="@Manager" />
        </ct>
        <x:if test="@IsSelfService != 'true'">
          <ct>
            <x:value-of select="$Res/@CourierColon" />
          </ct>
          <x:choose>
            <x:when test="@Courier != 'null' and @Courier != ''">
              <ct>
                <x:value-of select="@Courier" />
              </ct>
            </x:when>
            <x:otherwise>
              <c>
                <line symbols="_" />
              </c>
            </x:otherwise>
          </x:choose>
        </x:if>
      </cells>
    </table>
    <np />
    <table>
      <columns>
        <column autowidth="" />
        <column align="left" />
      </columns>
      <cells>
        <ct>
          <x:value-of select="$Res/@DeliveryCreatedColon" />
        </ct>
        <ct>
          <x:value-of select="iiko:FormatDateTimeCustom(@DeliveryReceiveTime, 'HH:mm dd.MM.yyyy')"/>
        </ct>
        <ct>
          <x:value-of select="$Res/@DeliveryBillPrintedColon" />
        </ct>
        <ct>
          <x:value-of select="iiko:FormatDateTimeCustom(@DeliveryBillPrintTime, 'HH:mm dd.MM.yyyy')"/>
        </ct>
        <ct>
          <x:value-of select="$Res/@DeliveryDateTimeColon" />
        </ct>
        <ct>
          <x:value-of select="iiko:FormatDateTimeCustom(@DeliverTime, 'HH:mm dd.MM.yyyy')"/>
        </ct>
        <ct>
          <x:value-of select="$Res/@DeliveryRealDateTimeColon" />
        </ct>
        <c>________________</c>
      </cells>
    </table>
    <np />
  </x:template>

  <x:template match="DeliveryOrderInfo" mode="Body">
    <x:param name="SplitBetweenPersons" />
    <x:param name="PersonsCount" />
    <left>
      <x:value-of select="iiko:Format($Res/@GuestsCountFormat, $PersonsCount)" />
    </left>
    <line />
    <x:apply-templates select="EntriesByGuests">
      <x:with-param name="SplitBetweenPersons" select="$SplitBetweenPersons"/>
    </x:apply-templates>
    <line />
    <x:if test="count(Discounts) > 0 or TotalSumsInfo/@VatSum != 0">
      <pair left="{$Res/@BillFooterTotalPlain}" right="{iiko:FormatMoney(TotalSumsInfo/@SubTotal)}" />
    </x:if>
    <x:if test="count(Discounts) > 0">      
      <x:apply-templates select="Discounts" />
    </x:if>
    <!-- vats -->
    <x:if test="TotalSumsInfo/@VatSum != 0">
      <x:for-each select="Vats/Vat">
        <pair left="{iiko:Format($Res/@VatFormat, @VatPercent)}" right="{iiko:FormatMoney(@VatSum)}" />
      </x:for-each>
      <x:if test="count(Vats/Vat) > 1">
        <pair left="{$Res/@VatSum}" right="{iiko:FormatMoney(TotalSumsInfo/@VatSum)}" />
      </x:if>
    </x:if>
    <!-- payments -->
    <pair left="{$Res/@BillFooterTotalLower}" right="{iiko:FormatMoney(TotalSumsInfo/@ResultSum)}" />
    <x:if test="/*/DeliveryInfo/@IsPayed = 'true'">
      <line />
      <x:for-each select="Prepays/Prepay">
        <pair left="{iiko:Format($Res/@PrepayTemplate, @Name)}" right="{iiko:FormatMoney(@Sum)}"/>
      </x:for-each>

      <x:for-each select="Payments/Payment[@IsCash='true']">
        <pair left="{$Res/@Cash}" right="{iiko:FormatMoney(@Sum)}"/>
      </x:for-each>
      <x:for-each select="Payments/Payment[@IsCard='true']">
        <pair left="{iiko:Format($Res/@CardPattern, @Name)}" right="{iiko:FormatMoney(@Sum)}"/>
      </x:for-each>
      <x:for-each select="Payments/Payment[@IsCard != 'true' and @IsCash != 'true']">
        <pair left="{@Name}" right="{iiko:FormatMoney(@Sum)}"/>
        <x:if test="@CounteragentName != 'null' and @CounteragentName != ''">
          <left>
            <x:value-of select="iiko:Format($Res/@CounteragentFormat, @CounteragentName)" />
          </left>
          <left>
            <x:value-of select="iiko:Format($Res/@CounteragentCardFormat, @CounteragentCardNumber)" />
          </left>
        </x:if>
      </x:for-each>
      <x:if test="TotalSumsInfo/@ChangeSum > 0">
        <pair left="{$Res/@Change}" right="{iiko:FormatMoney(TotalSumsInfo/@ChangeSum)}" />
      </x:if>
      <line />
      <pair left="{$Res/@FooterTotalUpper}" right="{iiko:FormatMoney(TotalSumsInfo/@ResultSum)}" />
    </x:if>

  </x:template>

  <x:template match="Discounts">
    <table>
      <columns>
        <column formatter="split" />
        <column align="right" autowidth="" />
        <column align="right" autowidth="" />
      </columns>
      <cells>
        <x:for-each select="Discount">
          <ct>
            <x:value-of select="@Name" />
          </ct>
          <ct>
            <x:value-of select="iiko:FormatPercent(@Percent)" />
          </ct>
          <ct>
            <x:value-of select="iiko:FormatMoney(@Sum)" />
          </ct>
          <x:if test="@CardNumber != 'null' and @CardNumber != ''">
            <c colspan="3">
              <x:value-of select="iiko:Format($Res/@CardPattern, @CardNumber)" />
            </c>
          </x:if>
        </x:for-each>
      </cells>
    </table>
  </x:template>

  <x:template match="EntriesByGuests">
    <x:param name="SplitBetweenPersons" />
    <table>
      <columns>
        <column formatter="split" />
        <column align="right" autowidth="" />
        <column align="right" autowidth="" />
        <column align="right" autowidth="" />
      </columns>
      <cells>
        <ct>
          <x:value-of select="$Res/@NameColumnHeader" />
        </ct>
        <ct>
          <x:value-of select="$Res/@AmountShort" />
        </ct>
        <ct>
          <x:value-of select="$Res/@ProductPrice" />
        </ct>
        <ct>
          <x:value-of select="$Res/@ResultSum" />
        </ct>
        <linecell />
        <x:if test="$SplitBetweenPersons = 'false'">
          <x:call-template name="DeliveryGuest">
            <x:with-param name="Guest" select="DeliveryGuest[1]" />
            <x:with-param name="SplitBetweenPersons" select="$SplitBetweenPersons" />
          </x:call-template>
        </x:if>
        <x:if test="$SplitBetweenPersons = 'true'">
          <x:call-template name="DeliveryGuest">
            <x:with-param name="Guest" select="DeliveryGuest[1]" />
            <x:with-param name="SplitBetweenPersons" select="$SplitBetweenPersons" />
          </x:call-template>

          <x:for-each select="DeliveryGuest[position() > 1]">
            <linecell symbols=" " />
            <x:call-template name="DeliveryGuest">
              <x:with-param name="Guest" select="." />
              <x:with-param name="SplitBetweenPersons" select="$SplitBetweenPersons" />
            </x:call-template>              
          </x:for-each>
        </x:if>
      </cells>
    </table>
  </x:template>
  
  <x:template name="DeliveryGuest">
    <x:param name="Guest"/>
    <x:param name="SplitBetweenPersons"/>
    <x:if test="$SplitBetweenPersons = 'true' and $Guest/@Name != ''">
      <c colspan="4">
        <x:value-of select="$Guest/@Name" />
      </c>
    </x:if>
    <x:apply-templates select="$Guest/DeliveryItems" />
    <x:if test="$SplitBetweenPersons = 'true' and $Guest/@Name != ''">
      <c colspan="3" />
      <c>
        <line />
      </c>
      <c colspan="2">
        <x:value-of select="iiko:Format($Res/@BillFooterTotalGuestPattern, $Guest/@Name)" />
      </c>
      <c colspan="2">
        <right>
          <x:value-of select="iiko:FormatMoney($Guest/@Sum)" />
        </right>
      </c>
    </x:if>
  </x:template>

  <x:template match="DeliveryItems">
      <x:apply-templates select="DeliveryOrderEntryInfo" />
  </x:template>

  <x:template match="DeliveryOrderEntryInfo">
    <c colspan="4">
      <x:choose>
        <x:when test="@IsModifier ='false'">
          <x:value-of select="@Name" />
        </x:when>
        <x:otherwise>
          <x:value-of select="iiko:Format('  {0}', @Name)" />
        </x:otherwise>
      </x:choose>
    </c>
    <c colspan="2">
      <right>
        <x:value-of select="concat(iiko:FormatAmount(@Amount), ' ', @MeasuringUnitName, ' x')" />
      </right>
    </c>
    <ct>
      <x:value-of select="iiko:FormatMoney(@Price)" />
    </ct>
    <ct>
      <x:value-of select="iiko:FormatMoney(@Sum)" />
    </ct>
    <x:for-each select="Discounts/Discount">
      <c colspan="3">
        <x:value-of select="iiko:Format('   {0} ({1})', @Name, iiko:FormatPercent(@Percent))" />
      </c>
      <ct>
        <x:value-of select="iiko:FormatMoney(@Sum)" />
      </ct>
      <x:if test="@CardNumber != ''">
        <c colspan="4">
          <x:value-of select="iiko:Format($Res/@CardPattern, @CardNumber)" />
        </c>
      </x:if>
    </x:for-each>
  </x:template>

  <!--Footer-->
  <x:template match="DeliveryInfo" mode="Footer">
    <np />
    <np />
    <split>
      <x:choose>
        <x:when test="@IsSelfService = 'true'">
          <x:value-of select="concat(iiko:Format($Res/@FreightDeliveriedFormat, @DeliveryOperator), '&#160;')" />
        </x:when>
        <x:otherwise>
          <x:value-of select="concat(iiko:Format($Res/@FreightDeliveriedFormat, @Courier), '&#160;')" />
        </x:otherwise>
      </x:choose>
      <line symbols="_" />
    </split>
    <np />
    <split>
      <x:value-of select="concat($Res/@FreightReceivedColon, '&#160;')" />
      <line symbols="_" />
    </split>
    <np />
    <left>
      <x:value-of select="$Res/@DeliveryProcessingCommentsColon" />
    </left>
    <x:if test="@Comments != 'null' and @Comments != ''">
      <left>
        <x:value-of select="@Comments" />
      </left>
    </x:if>
    <line symbols="_" />
    <line symbols="_" />
    <line symbols="_" />
    <line symbols="_" />
    <np />
    <np />
    <center>
      <x:value-of select="/*/DeliveryOrderInfo/TotalSumsInfo/@AllSumsCurrencyUnitLabel"/>
    </center>
    <np />
    <center>
      <x:value-of select="/*/BillFooter/@Value" />
    </center>
  </x:template>


</x:stylesheet>