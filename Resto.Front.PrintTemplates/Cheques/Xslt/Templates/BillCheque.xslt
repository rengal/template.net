<?xml version="1.0" encoding="utf-8"?>
<x:stylesheet version="1.0" xmlns:x="http://www.w3.org/1999/XSL/Transform">
  <x:output method="xml" version="1.0" encoding="utf-8" indent="yes" />

  <x:variable name="Res" select="/*/Settings/Resources" />
  <!-- ChequeLocalResources (strings as attrs) -->

  <!-- Пречек -->
  <x:template match="/Bill">
    <doc>
      <!-- Header (begin) -->
      <x:copy-of select="Extensions/BeforeHeader/*" />
      <left>
        <split>
          <x:value-of select="Settings/@ChequeHeader" />
        </split>
      </left>

      <x:if test="@IsAdditionalServiceCheque = 'false'">
        <center>
          <x:value-of select="$Res/@BillHeaderTitle" />
        </center>
      </x:if>
      <pair fit="right" left="{iiko:Format($Res/@BillHeaderSectionPattern, @SectionName)}" right="{iiko:Format($Res/@BillHeaderTablePattern, @TableNumber)}" />
      <pair fit="right" left="{iiko:Format($Res/@BillHeaderOrderOpenPattern, iiko:FormatLongDateTime(@OrderOpenTime))}" right="{iiko:Format($Res/@BillHeaderOrderNumberPattern, @OrderNumber)}" />
      <x:if test="@IsAdditionalServiceCheque = 'true'">
        <left>
          <x:value-of select="iiko:Format($Res/@AdditionalServiceHeaderOrderItemsAddedPattern, iiko:FormatLongDateTime(Settings/@Now))" />
        </left>
      </x:if>

      <left>
        <x:value-of select="iiko:Format($Res/@BillHeaderWaiterPattern, @WaiterName)" />
      </left>

      <x:for-each select="GuestCardsInfo/Value">
        <left>
          <x:value-of select="iiko:Format($Res/@ClientFormat, .)" />
        </left>
      </x:for-each>
      <x:copy-of select="Extensions/AfterHeader/*" />
      <x:if test="@IsAdditionalServiceCheque = 'true'">
        <x:if test="AdditionalServiceInfo/@ClientBindCardInfo != ''">
          <left>
            <x:value-of select="iiko:Format($Res/@CardPattern, AdditionalServiceInfo/@ClientBindCardInfo)" />
          </left>
        </x:if>
        <np/>
        <center>
          <x:value-of select="$Res/@AdditionalServiceHeaderTitle" />
        </center>
      </x:if>
      <!-- Header (end) -->

      <!-- Body (begin) -->
      <table>
        <columns>
          <column />
          <column align="right" autowidth="" />
          <column align="right" autowidth="" />
        </columns>
        <cells>
          <x:if test="count(Guests/Guest/Products/Product) > 0">
            <linecell />
            <ct>
              <x:value-of select="$Res/@NameColumnHeader" />
            </ct>
            <ct>
              <x:value-of select="$Res/@ProductAmount" />
            </ct>
            <ct>
              <x:value-of select="$Res/@ResultSum" />
            </ct>
            <linecell />
          </x:if>

          <x:if test="count(Guests/Guest) = 1">
            <x:call-template name="BodyTableForSingleGuest">
              <x:with-param name="Guest" select="Guests/Guest[1]" />
            </x:call-template>
          </x:if>

          <x:if test="count(Guests/Guest) > 1">
            <x:call-template name="PartOfTableForOneOfMultipleGuests">
              <x:with-param name="Guest" select="Guests/Guest[1]" />
              <x:with-param name="IsAdditionalServiceCheque" select="@IsAdditionalServiceCheque" />
            </x:call-template>

            <x:for-each select="Guests/Guest[position() > 1]">
              <linecell symbols=" " />
              <x:call-template name="PartOfTableForOneOfMultipleGuests">
                <x:with-param name="Guest" select="." />
                <x:with-param name="IsAdditionalServiceCheque" select="../../@IsAdditionalServiceCheque" />
              </x:call-template>
            </x:for-each>
          </x:if>

          <linecell />

          <x:if test="(@Prepay != 0 or @FullSum != @Total) and @IsAdditionalServiceCheque = 'false'">
            <c colspan="2">
              <x:value-of select="$Res/@BillFooterFullSum" />
            </c>
            <ct>
              <x:value-of select="iiko:FormatMoney(@FullSum)" />
            </ct>
          </x:if>

          <x:for-each select="CategorizedDiscountsAndIncreases/DiscountIncrease">
            <c colspan="2">
              <x:call-template name="DiscountIncreaseDescription" />
            </c>
            <ct>
              <x:call-template name="DiscountIncreaseSum" />
            </ct>
          </x:for-each>

          <x:if test="count(CategorizedDiscountsAndIncreases/DiscountIncrease) > 0">
            <c colspan="2">
              <x:value-of select="$Res/@BillFooterTotalPlain" />
            </c>
            <ct>
              <x:value-of select="iiko:FormatMoney(@SubTotal)" />
            </ct>
          </x:if>

          <x:for-each select="NonCategorizedDiscountsAndIncreases/DiscountIncrease">
            <c colspan="2">
              <x:call-template name="DiscountIncreaseDescription" />
            </c>
            <ct>
              <x:call-template name="DiscountIncreaseSum" />
            </ct>
          </x:for-each>

          <x:if test="count(DiscountMarketingCampaigns/DiscountMarketingCampaign) > 0">
            <x:for-each select="DiscountMarketingCampaigns/DiscountMarketingCampaign">
              <c colspan="2">
                <x:value-of select="@Name" />
              </c>
              <ct>
                <x:text>-</x:text>
                <x:value-of select="iiko:FormatMoney(@TotalDiscount)" />
              </ct>
            </x:for-each>
          </x:if>

          <x:if test="(count(CategorizedDiscountsAndIncreases/DiscountIncrease) > 0 or
                       count(NonCategorizedDiscountsAndIncreases/DiscountIncrease) > 0) and
                      @Prepay != 0">
            <c colspan="2">
              <x:value-of select="$Res/@BillFooterTotalWithoutDiscounts" />
            </c>
            <ct>
              <x:value-of select="iiko:FormatMoney(@TotalWithoutDiscounts)" />
            </ct>
          </x:if>

          <x:if test="@VatSum != 0">
            <x:for-each select="Vats/VatItem">
              <c colspan="2">
                <x:value-of select="iiko:Format($Res/@VatFormat, @VatPercent)" />
              </c>
              <ct>
                <x:value-of select="iiko:FormatMoney(@VatSum)" />
              </ct>
            </x:for-each>
            <x:if test="count(Vats/VatItem) > 1">
              <c colspan="2">
                <x:value-of select="$Res/@VatSum" />
              </c>
              <ct>
                <x:value-of select="iiko:FormatMoney(@VatSum)" />
              </ct>
            </x:if>
          </x:if>

          <x:if test="@IsAdditionalServiceCheque = 'true'">
            <c colspan="2">
              <x:value-of select="$Res/@AdditionalServiceAddedFooterTotalUpper" />
            </c>
            <ct>
              <x:value-of select="iiko:FormatMoney(AdditionalServiceInfo/@AdditionalSum)" />
            </ct>
          </x:if>

          <x:if test="@Prepay != 0">
            <c colspan="2">
              <x:value-of select="$Res/@Prepay" />
            </c>
            <ct>
              <x:value-of select="iiko:FormatMoney(@Prepay)" />
            </ct>
          </x:if>

          <c colspan="2">
            <x:if test="@IsAdditionalServiceCheque = 'false'">
              <x:value-of select="$Res/@BillFooterTotal" />
            </x:if>
            <x:if test="@IsAdditionalServiceCheque = 'true'">
              <x:value-of select="$Res/@AdditionalServiceFooterTotalUpper" />
            </x:if>
          </c>
          <ct>
            <x:value-of select="iiko:FormatMoney(@Total)" />
          </ct>

          <x:if test="@IsAdditionalServiceCheque = 'true' and AdditionalServiceInfo/@HasLimitSum = 'true'">
            <c colspan="2">
              <x:value-of select="$Res/@AdditionalServiceLimit" />
            </c>
            <ct>
              <x:value-of select="iiko:FormatMoney(AdditionalServiceInfo/@LimitLeftSum)" />
            </ct>
          </x:if>

          <x:if test="count(DiscountMarketingCampaigns/DiscountMarketingCampaign) > 0">
            <x:for-each select="DiscountMarketingCampaigns/DiscountMarketingCampaign">
              <x:if test="@BillComment != ''">
                <c colspan="3">
                  <x:value-of select="@BillComment" />
                </c>
              </x:if>
            </x:for-each>
          </x:if>
        </cells>
      </table>
      <!-- Body (end) -->

      <!-- Footer (begin) -->
      <np />
      <x:copy-of select="Extensions/BeforeFooter/*" />
      <center>
        <split>
          <x:value-of select="Settings/@ChequeFooter" />
        </split>
      </center>
      <np />
      <np />
      <x:if test="@PrintSignature = 'true'">
        <np />
        <line />
        <center>
          <x:value-of select="$Res/@Signature"/>
        </center>
      </x:if>
      <x:copy-of select="Extensions/AfterFooter/*" />
      <np />
      <!-- Footer (end) -->
    </doc>
  </x:template>

  <x:template name="BodyTableForSingleGuest">
    <x:param name="Guest" />
    <x:for-each select="$Guest/Products/Product">
      <ct>
        <x:value-of select="@Name" />
      </ct>
      <ct>
        <x:value-of select="iiko:FormatAmount(@Amount)" />
      </ct>
      <ct>
        <x:value-of select="iiko:FormatMoney(@Sum)" />
      </ct>

      <x:for-each select="CategorisedDiscountsAndIncreases/DiscountIncrease">
        <c colspan="2">
          <x:call-template name="DiscountIncreaseDescriptionForOrderItem" />
        </c>
        <ct>
          <x:call-template name="DiscountIncreaseSum" />
        </ct>
      </x:for-each>

      <x:if test="@CommentExists = 'true'">
        <c>
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
        <c colspan="2" />
      </x:if>

      <x:for-each select="Modifiers/Modifier">
        <ct>
          <x:text xml:space="preserve">  </x:text>
          <x:value-of select="@Name" />
        </ct>
        <x:choose>
          <x:when test="@Amount != 1">
            <ct>
              <x:value-of select="iiko:FormatAmount(@Amount)" />
            </ct>
          </x:when>
          <x:otherwise>
            <ct />
          </x:otherwise>
        </x:choose>
        <x:choose>
          <x:when test="@Sum != 0">
            <ct>
              <x:value-of select="iiko:FormatMoney(@Sum)" />
            </ct>
          </x:when>
          <x:otherwise>
            <ct />
          </x:otherwise>
        </x:choose>
        <x:for-each select="CategorisedDiscountsAndIncreases/DiscountIncrease">
          <c colspan="2">
            <x:call-template name="DiscountIncreaseDescriptionForOrderItem" />
          </c>
          <ct>
            <x:call-template name="DiscountIncreaseSum" />
          </ct>
        </x:for-each>
      </x:for-each>
    </x:for-each>
  </x:template>

  <x:template name="PartOfTableForOneOfMultipleGuests">
    <x:param name="Guest" />
      <x:param name="IsAdditionalServiceCheque" />

    <c colspan="0">
      <x:value-of select="$Guest/@Name" />
    </c>
    <x:call-template name="BodyTableForSingleGuest">
      <x:with-param name="Guest" select="$Guest" />
    </x:call-template>

    <c colspan="2" />
    <c>
      <line />
    </c>

    <x:if test="$Guest/@Subtotal != $Guest/@Total">
      <c colspan="2">
        <x:value-of select="$Res/@BillFooterTotalPlain" />
      </c>
      <ct>
        <x:value-of select="iiko:FormatMoney($Guest/@Subtotal)" />
      </ct>

      <x:for-each select="$Guest/NonCategorisedDiscountsAndIncreases/DiscountIncrease">
        <c colspan="2">
          <x:call-template name="DiscountIncreaseDescription" />
        </c>
        <ct>
          <x:call-template name="DiscountIncreaseSum" />
        </ct>
      </x:for-each>
    </x:if>

    <c colspan="2">
      <x:choose>
        <x:when test="$IsAdditionalServiceCheque = 'true'">
          <x:value-of select="iiko:Format($Res/@AdditionalServiceFooterTotalGuestPattern, $Guest/@Name)" />
        </x:when>
        <x:otherwise>
          <x:value-of select="iiko:Format($Res/@BillFooterTotalGuestPattern, $Guest/@Name)" />
        </x:otherwise>
      </x:choose>
    </c>
    <ct>
      <x:value-of select="iiko:FormatMoney($Guest/@Total)" />
    </ct>
  </x:template>

  <x:template name="DiscountIncreaseDescriptionForOrderItem">
    <x:if test="@PrintDetailed">
      <x:text xml:space="preserve">  </x:text>
      <x:choose>
        <x:when test="@IsDiscount = 'true'">
          <x:value-of select="iiko:Format('{0} (-{1})', @Name, iiko:FormatPercent(@Percent))" />
        </x:when>
        <x:when test="@IsDiscount = 'false'">
          <x:value-of select="iiko:Format('{0} (+{1})', @Name, iiko:FormatPercent(@Percent))" />
        </x:when>
      </x:choose>
    </x:if>
  </x:template>

  <x:template name="DiscountIncreaseDescription">
    <x:choose>
      <x:when test="@PrintDetailed = 'true' and @IsDiscount = 'true'">
        <x:value-of select="iiko:Format($Res/@BillFooterDiscountNamePatternDetailed, @Name, iiko:FormatPercent(@Percent))" />
      </x:when>
      <x:when test="@PrintDetailed = 'true' and @IsDiscount = 'false'">
        <x:value-of select="iiko:Format($Res/@BillFooterIncreaseNamePatternDetailed, @Name, iiko:FormatPercent(@Percent))" />
      </x:when>
      <x:when test="@PrintDetailed = 'false' and @IsDiscount = 'true'">
        <x:value-of select="iiko:Format($Res/@BillFooterDiscountNamePatternShort, @Name)" />
      </x:when>
      <x:when test="@PrintDetailed = 'false' and @IsDiscount = 'false'">
        <x:value-of select="iiko:Format($Res/@BillFooterIncreaseNamePatternShort, @Name)" />
      </x:when>
    </x:choose>
  </x:template>

  <x:template name="DiscountIncreaseSum">
    <x:choose>
      <x:when test="@IsDiscount = 'true'">
        <x:value-of select="concat('-', iiko:FormatMoney(@Sum))" />
      </x:when>
      <x:when test="@IsDiscount = 'false'">

        <x:value-of select="concat('+', iiko:FormatMoney(@Sum))" />
      </x:when>
    </x:choose>
  </x:template>

</x:stylesheet>
