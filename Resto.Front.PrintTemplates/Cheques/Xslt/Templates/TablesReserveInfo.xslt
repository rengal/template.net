<?xml version="1.0" encoding="utf-8"?>
<x:stylesheet version="1.0" xmlns:x="http://www.w3.org/1999/XSL/Transform">
  <x:output method="xml" version="1.0" encoding="utf-8" indent="yes" />

  <x:variable name="Res" select="/*/Settings/Resources" />
  <!-- ChequeLocalResources (strings as attrs) -->

  <x:template match="/TablesReserve">
    <x:variable name="StartDate" select="substring(/*/@StartTime, 1, 10)" />
    <x:variable name="EndDate" select="substring(/*/@EndTime, 1, 10)" />

    <doc>
      <center>
        <x:value-of select="$Res/@TablesReserveHeaderTitle" />
      </center>
      <np />
      <center>
        <x:value-of select="iiko:Format($Res/@ReserveNumberFormat, @Type, @Number)" />
      </center>
      <np />
      <pair fit="right" left="{iiko:FormatDate(@StartTime)}" right="{@StartDayOfWeekName}" />
      <f1>
        <left>
          <x:choose>
            <!-- Даты совпадают => пишем только время -->
            <x:when test="$StartDate = $EndDate">
              <x:value-of select="iiko:Format($Res/@ReserveDurationFormat, iiko:FormatTime(@StartTime), iiko:FormatTime(@EndTime))"/>
            </x:when>
            <!-- иначе пишем полностью дату и время -->
            <x:otherwise>
              <x:value-of select="iiko:Format($Res/@ReserveDurationFormat, iiko:FormatLongDateTime(@StartTime), iiko:FormatLongDateTime(@EndTime))"/>
            </x:otherwise>
          </x:choose>
        </left>
      </f1>
      <np />

      <x:for-each select="Sections/Section">
        <left>
          <x:value-of select="iiko:Format($Res/@ReservedSectionTablesFormat, @Name, @Tables)" />
        </left>
      </x:for-each>

      <np />
      <left>
        <x:value-of select="iiko:Format($Res/@GuestsCountFormat, @GuestsCount)"/>
      </left>
      <x:if test="@ActivityType != 'null' and @ActivityType != ''">
        <left>
          <x:value-of select="iiko:Format($Res/@ActivityTypeFormat, @ActivityType)"/>
        </left>
      </x:if>
      <x:if test="@Comment != 'null' and @Comment != ''">
        <left>
          <x:value-of select="iiko:Format($Res/@ReserveCommentFormat, @Comment)"/>
        </left>
      </x:if>
      <np />
      <np />
      <line />
      <np />
      <np />
      <left>
        <x:value-of select="iiko:Format($Res/@ReserveCustomerNameFormat, Customer/@Name, Customer/@Surname)"/>
      </left>
      <x:for-each select="Customer/PhoneNumbers/Value">
        <left>
          <x:value-of select="iiko:Format($Res/@ReserveCustomerPhoneNumbersFormat, .)" />
        </left>
      </x:for-each>
      <x:if test="Customer/@DiscountCardType != 'null' and Customer/@DiscountCardType != ''">
        <left>
          <x:value-of select="iiko:Format($Res/@DiscountCardTypeNameFormat, Customer/@DiscountCardType)"/>
        </left>
      </x:if>
      <x:if test="Customer/@Comment != 'null' and Customer/@Comment != ''">
        <np />
        <left>
          <x:value-of select="Customer/@Comment" />
        </left>
      </x:if>
    </doc>
  </x:template>
</x:stylesheet>
