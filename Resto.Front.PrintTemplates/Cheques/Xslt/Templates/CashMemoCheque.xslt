<?xml version="1.0" encoding="utf-8"?>
<x:stylesheet version="1.0" xmlns:x="http://www.w3.org/1999/XSL/Transform">
  <x:output method="xml" version="1.0" encoding="utf-8" indent="yes" />

  <!-- ChequeLocalResources (strings as attrs) -->
  <x:variable name="Res" select="/*/Settings/Resources" />

  <!-- Чек: товарный чек -->
  <x:template match="/CashMemo">
    <doc>
      <!-- header -->
      <left>
        <split>
          <x:value-of select="Settings/@ChequeHeader" />
        </split>
      </left>
      <center>
        <x:value-of select="$Res/@CashMemoChequeTitle" />
      </center>
      <pair left="{iiko:Format($Res/@CashRegisterFormat, Info/@CashRegisterNumber)}" right="{Settings/@GroupName}" />
      <pair left="{$Res/@HeadCashRegisterShift}" right="{Info/@CafeSessionNumber}" />
      <pair left="{$Res/@CafeSessionOpenDate}" right="{iiko:FormatLongDateTime(Info/@CafeSessionOpenTime)}" />
      <pair left="{$Res/@CurrentDate}" right="{iiko:FormatLongDateTime(Info/@OperationTime)}" />
      <pair left="{iiko:Format($Res/@BillHeaderCashierPattern, Info/@CashierName)}" right="{iiko:Format($Res/@BillHeaderOrderNumberPattern, @OrderNumber)}" />

      <!-- body -->
      <line />
      <x:apply-templates select="Products" />
      <line />
      <x:if test="count(Discounts) > 0">
        <pair left="{$Res/@BillFooterTotalPlain}" right="{iiko:FormatMoney(@SubTotal)}" />
        <x:apply-templates select="Discounts" />
      </x:if>
      <x:if test="@VatSum != 0">
        <x:for-each select="Vats/Vat">
          <pair left="{iiko:Format($Res/@VatFormat, @VatPercent)}" right="{iiko:FormatMoney(@VatSum)}" />
        </x:for-each>
        <x:if test="count(Vats/Vat) > 1">
          <pair left="{$Res/@VatSum}" right="{iiko:FormatMoney(@VatSum)}" />
        </x:if>
      </x:if>

      <!-- payments -->
      <pair left="{$Res/@BillFooterTotalLower}" right="{iiko:FormatMoney(@ResultSum)}" />
      <line />
      <x:for-each select="Prepays/Prepay">
        <pair left="{iiko:Format($Res/@PrepayTemplate, @Name)}" right="{iiko:FormatMoney(@Sum)}"/>
      </x:for-each>
      <x:for-each select="Payments/Payment">
        <pair left="{@Name}" right="{iiko:FormatMoney(@Sum)}"/>
      </x:for-each>
      <line />
      <x:value-of select="iiko:Format($Res/@FooterTotalUpper, iiko:FormatMoney(@ResultSum))" />

      <!-- footer -->
      <pair left="{$Res/@ItemsCount}" right="{@ItemsCount}"/>
      <left>
        <x:value-of select="iiko:Format($Res/@SumFormat, iiko:FormatMoneyInWords(@ResultSum))" />
      </left>
      <np />
      <left>
        <x:value-of select="$Res/@Released" />
      </left>
      <fill symbols="_">
        <np />
      </fill>
      <center>
        <x:value-of select="$Res/@MemoChequeSignature" />
      </center>
      <np />
      <center>
        <x:value-of select="iiko:Format($Res/@AllSumsInFormat, Settings/@CurrencyName)" />
      </center>
      <np />
      <center>
        <split>
          <x:value-of select="Settings/@ChequeFooter" />
        </split>
      </center>
    </doc>
  </x:template>

  <!-- Products list -->
  <x:template match="Products">
    <x:choose>
      <x:when test="count(Product) = 0">
        <x:value-of select="$Res/@ZeroChequeBody" />
      </x:when>
      <x:otherwise>
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
            <x:apply-templates select="Product" />
          </cells>
        </table>
      </x:otherwise>
    </x:choose>
  </x:template>

  <x:template match="Product">
    <c colspan="4">
      <x:value-of select="@Name" />
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
    <x:if test="@CommentExists = 'true'">
      <c colspan="4">
        <table cellspacing="0">
          <columns>
            <column width="2" />
            <column />
          </columns>
          <cells>
            <c />
            <c>
              <split>
                <x:value-of select="@Comment" />
              </split>
            </c>
          </cells>
        </table>
      </c>
    </x:if>
    <x:for-each select="Discounts/Discount">
      <c colspan="3">
        <x:value-of select="iiko:Format($Res/@CashMemoChequeDiscountItemNameAndPercentFormat, @Name, iiko:FormatPercent(@Percent))" />
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
    <x:for-each select="Modifiers/Modifier">
      <c colspan="4">
        <x:value-of select="iiko:Format('  {0}', @Name)" />
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
    </x:for-each>
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
          <x:if test="@CardNumber != ''">
            <c colspan="3">
              <x:value-of select="iiko:Format($Res/@CardPattern, @CardNumber)" />
            </c>
          </x:if>
        </x:for-each>
      </cells>
    </table>
  </x:template>
</x:stylesheet>
