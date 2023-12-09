<?xml version="1.0" encoding="utf-8"?>
<x:stylesheet version="1.0" xmlns:x="http://www.w3.org/1999/XSL/Transform">
  <x:output method="xml" version="1.0" encoding="utf-8" indent="yes" />

  <x:variable name="Res" select="/*/Settings/Resources" />
  <!-- ChequeLocalResources (strings as attrs) -->

  <!-- Этикетка -->
  <x:template match="/PriceTicket">
    <doc>
      <center>
        <x:value-of select="Settings/@EnterpriseName"/>
      </center>
      <f1>
        <center>
          <x:value-of select="@ProductName"/>
        </center>
      </f1>
      <left>
        <wrap>
          <x:value-of select="$Res/@BarcodeAmount" />
          <x:text xml:space="preserve"> </x:text>
          <x:value-of select="iiko:FormatAmount(@Amount)" />
          <x:text xml:space="preserve"> </x:text>
          <x:value-of select="@UnitName" />
          <x:text xml:space="preserve">  </x:text>
          <x:value-of select="$Res/@BarcodePrice" />
          <x:text xml:space="preserve"> </x:text>
          <x:value-of select="iiko:FormatMoney(@Price)" />
          <x:text xml:space="preserve"> </x:text>
          <x:value-of select="Settings/@ShortCurrencyName"/>
        </wrap>
      </left>
      <left>
        <x:value-of select="$Res/@BarcodeSum" />
        <x:text xml:space="preserve"> </x:text>
        <x:value-of select="iiko:FormatMoney(@Cost)" />
        <x:text xml:space="preserve"> </x:text>
        <x:value-of select="Settings/@ShortCurrencyName"/>
      </left>
      <left>
        <x:if test="@IsExpirationPeriodSet = 'true'">
          <x:value-of select="$Res/@BarcodeExpiration" />
          <x:text xml:space="preserve"> </x:text>
          <x:value-of select="iiko:FormatLongDateTime(@BestBeforeTime)" />
        </x:if>
      </left>
      <x:if test="@BarcodeString">
        <left>
          <barcode>
            <x:value-of select="@BarcodeString"/>
          </barcode>
        </left>    
      </x:if>
    </doc>
  </x:template>
</x:stylesheet>
