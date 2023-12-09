<?xml version="1.0" encoding="utf-8"?>
<x:stylesheet version="1.0" xmlns:x="http://www.w3.org/1999/XSL/Transform">
  <x:output method="xml" version="1.0" encoding="utf-8" indent="yes" />

  <x:variable name="Res" select="/*/Settings/Resources" />
  <!-- ChequeLocalResources (strings as attrs) -->

  <!-- Стикер -->
  <x:template match="/DeliverySticker">
    <doc>
      <center>
        <x:value-of select="Settings/@EnterpriseName"/>
      </center>
      <left>
        <wrap>
          <x:value-of select="iiko:Format($Res/@BillHeaderOrderNumberPattern, @DeliveryNumber)" />
          <x:text xml:space="preserve">   </x:text>
          <x:value-of select="OrderItem/@GuestName" />
        </wrap>
      </left>
      <left>
        <wrap>
          <x:value-of select="$Res/@DeliveryDateTimeColon" />
          <x:text xml:space="preserve"> </x:text>
          <x:value-of select="iiko:FormatLongDateTime(@DeliverTime)" />
        </wrap>
      </left>
      <left>
        <wrap>
          <x:value-of select="OrderItem/@Name" />
          <x:if test="OrderItem/@UseBalanceForSell = 'true' and OrderItem/@Amount != 1">
            <x:text xml:space="preserve"> x </x:text>
            <x:value-of select="iiko:FormatAmount(OrderItem/@Amount)" />
          </x:if>
          <x:variable name="ModsCount" select="count(OrderItem/Modifiers/Modifier)"/>
          <x:if test="$ModsCount > 0">
            <x:text xml:space="preserve"> (</x:text>
            <x:for-each select="OrderItem/Modifiers/Modifier">
              <x:value-of select="@Name" />
              <x:if test="@Amount != 1">
                <x:text xml:space="preserve"> x </x:text>
                <x:value-of select="iiko:FormatAmount(@Amount)" />
              </x:if>
              <x:if test="position() &lt; $ModsCount">
                <x:text xml:space="preserve">; </x:text>
              </x:if>
            </x:for-each>
            <x:text xml:space="preserve">)</x:text>
          </x:if>
        </wrap>
      </left>
      <left>
        <wrap>
          <x:value-of select="$Res/@ProductPriceColon" />
          <x:text xml:space="preserve"> </x:text>
          <x:value-of select="iiko:FormatMoney(OrderItem/@Price)" />
        </wrap>
      </left>
      <x:if test="OrderItem/@CategoryName != ''">
        <left>
          <wrap>
            <x:value-of select="$Res/@CategoryColon" />
            <x:text xml:space="preserve"> </x:text>
            <x:value-of select="OrderItem/@CategoryName" />
          </wrap>
        </left>
      </x:if>
      <x:if test="OrderItem/@Composition != ''">
        <left>
          <wrap>
            <x:value-of select="OrderItem/@Composition" />
          </wrap>
        </left>
      </x:if>
      <x:if test="OrderItem/FoodValue/@IsEmpty = 'false'">
        <left>
          <wrap>
            <x:value-of select="iiko:Format($Res/@ProteinFormat, iiko:FormatFoodValueItem(OrderItem/FoodValue/@Protein))" />
            <x:text xml:space="preserve"> </x:text>
            <x:value-of select="iiko:Format($Res/@FatFormat, iiko:FormatFoodValueItem(OrderItem/FoodValue/@Fat))" />
            <x:text xml:space="preserve"> </x:text>
            <x:value-of select="iiko:Format($Res/@CarbohydrateFormat, iiko:FormatFoodValueItem(OrderItem/FoodValue/@Carbohydrate))" />
            <x:text xml:space="preserve"> </x:text>
            <x:value-of select="iiko:Format($Res/@CaloricityFormat, iiko:FormatFoodValueItem(OrderItem/FoodValue/@Caloricity))" />
          </wrap>
        </left>
      </x:if>
      <left>
        <x:if test="OrderItem/@IsExpirationPeriodSet = 'true'">
          <x:value-of select="iiko:Format($Res/@ExpirationPeriodFormat, OrderItem/@ExpirationPeriodHours)" />
          <np />
        </x:if>
        <x:value-of select="$Res/@PrintTimeColon" />
        <x:text xml:space="preserve"> </x:text>
        <x:value-of select="iiko:FormatLongDateTime(OrderItem/@PrintTime)" />
      </left>
      <np />
    </doc>
  </x:template>
</x:stylesheet>
