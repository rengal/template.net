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

  <!-- Чек: сервисный чек -->
  <x:template match="/Service">
    <doc bell="" formatter="split">
      <x:call-template name="HeaderCommon" />
      <x:apply-templates select="CourseGroups" />
      <x:call-template name="FooterCommon" />
    </doc>
  </x:template>

  <!-- Чек: чек заверешния готовки -->
  <x:template match="/CookingCompleted">
    <doc bell="" formatter="split">
      <x:call-template name="HeaderCommon" />
      <x:apply-templates select="CourseGroups" />
      <x:call-template name="FooterCommon" />
    </doc>
  </x:template>

  <!-- Чек: заказ на банкет -->
  <x:template match="/BanquetService">
    <doc bell="" formatter="split">
      <x:value-of select="iiko:Format($Res/@BanquetNumberServiceChequeHeaderTemplate, @BanquetNumber)" />
      <np />
      <x:value-of select="iiko:Format($Res/@KitchenHeaderPattern, iiko:FormatFullDateTime(Settings/@Now), Info/Order/@Number, Info/@PrinterCounter, Info/Order/@WaiterName)" />
      <np />
      <np />
      <x:call-template name="HeaderTableCommon" />
      <x:apply-templates select="CourseGroups" />
      <x:call-template name="FooterCommon" />
    </doc>
  </x:template>

  <!-- Чек: удаление продуктов -->
  <x:template match="/DeleteProducts">
    <doc bell="" formatter="split">
      <x:call-template name="HeaderCommon" />
      <f2>
        <x:value-of select="iiko:Format($Res/@KitchenProductDeletedWarningPattern, @DeleteReason)" />
      </f2>
      <np />
      <np />
      <x:apply-templates select="CourseGroups" />
      <x:call-template name="FooterCommon" />
    </doc>
  </x:template>

  <!-- Чек: удаление модификаторов -->
  <x:template match="/DeleteModifiers">
    <doc bell="" formatter="split">
      <x:call-template name="HeaderCommon" />
      <f2>
        <x:value-of select="iiko:Format($Res/@KitchenModifiersDeletedWarningPattern, @DeleteReason)" />
      </f2>
      <np />
      <np />
      <x:apply-templates select="CourseGroups" />
      <x:call-template name="FooterCommon" />
    </doc>
  </x:template>

  <!-- Чек: подача блюд -->
  <x:template match="/ProductsServe">
    <doc bell="" formatter="split">
      <x:call-template name="HeaderCommon" />
      <f2>
        <x:value-of select="$Res/@TimeToServeDishes" />
      </f2>
      <np />
      <x:apply-templates select="CourseGroups" />
      <x:call-template name="FooterCommon" />
    </doc>
  </x:template>

  <!-- Чек: подача целого курса -->
  <x:template match="/WholeCourseServe">
    <doc bell="" formatter="split">
      <x:value-of select="iiko:Format($Res/@KitchenHeaderPattern, iiko:FormatFullDateTime(Settings/@Now), Order/@Number, @PrinterCounter, Info/Order/@WaiterName)" />
      <np />
      <np />
      <f1>
        <x:value-of select="iiko:Format($Res/@KitchenTablePattern, Order/@TableNumber, Order/@SectionName)" />
      </f1>
      <np />
      <np />
      <f2>
        <x:value-of select="iiko:Format($Res/@TimeToServeCourseTemplate, @Course)" />
      </f2>
      <np />
      <x:value-of select="$Res/@SeeDetailsInCookingRequestText" />
      <np />
      <x:call-template name="FooterCommon" />
    </doc>
  </x:template>

  <!-- Cheque items list (mixed parts) -->
  <x:template match="CourseGroup">
    <f1>
      <np />
    </f1>
    <x:if test="/*/Info/@HasCourses = 'true'">
      <x:variable name="VipTitlePrefix">
        <x:if test="name(/*) = 'Service' or name(/*) = 'BanquetService'">
          <x:value-of select="$Res/@KitchenCourseVipTitlePrefix" />
        </x:if>
      </x:variable>
      <f1>
        <x:choose>
          <x:when test="@Course = 0">
            <x:value-of select="concat($VipTitlePrefix, ' ', $Res/@KitchenCourseVipTitle)" />
          </x:when>
          <x:when test="@Course = 1">
            <x:value-of select="$Res/@KitchenCourse1Title" />
          </x:when>
          <x:when test="@Course = 2">
            <x:value-of select="$Res/@KitchenCourse2Title" />
          </x:when>
          <x:when test="@Course = 3">
            <x:value-of select="$Res/@KitchenCourse3Title" />
          </x:when>
          <x:when test="@Course = 4">
            <x:value-of select="$Res/@KitchenCourse4Title" />
          </x:when>
          <x:when test="@Course > 4">
            <x:value-of select="iiko:Format($Res/@KitchenCourseTitleFormat, @Course)" />
          </x:when>
        </x:choose>
      </f1>
    </x:if>
    <x:apply-templates select="ChequeItems/ChequeItem" />
  </x:template>

  <x:template match="ChequeItem">
    <x:apply-templates select="Products" />
    <x:if test="IsWholeMix = 'false'">
      <x:call-template name="PartOfMixRow" />
    </x:if>
  </x:template>

  <x:template match="ChequeItem[name(/*) = 'DeleteModifiers']">
    <x:choose>
      <x:when test="count(Products/Product) = 1 and count(Products/Product[1]/Modifiers/Modifier) = 0">
        <f1>
          <x:apply-templates select="Products" />
        </f1>
      </x:when>
      <x:otherwise>
        <x:apply-templates select="Products/Product[1]/Modifiers" />
        <f0>
          <np />
          <center>
            <x:value-of select="$Res/@ForDishTitle" />
          </center>
          <np />
        </f0>
        <x:apply-templates select="Products/Product[1]" />
        <x:if test="IsWholeMix = 'false'">
          <x:call-template name="PartOfMixRow" />
        </x:if>
      </x:otherwise>
    </x:choose>
  </x:template>

  <!-- Modifiers list -->
  <x:template match="Modifiers">
    <f0>
      <table>
        <x:call-template name="OrderItemTableColumns" />
        <cells>
          <x:apply-templates select="Modifier" />
        </cells>
      </table>
    </f0>
  </x:template>

  <x:template match="Modifier">
    <ct>
      <x:value-of select="@Code" />
    </ct>
    <c>
      <x:choose>
        <x:when test="@CookingPlaceName">
          <x:value-of select="iiko:Format($Res/@CookingPlaceTemplate, @CookingPlaceName, @Name)" />
        </x:when>
        <x:otherwise>
          <x:value-of select="@Name" />
        </x:otherwise>
      </x:choose>
    </c>
    <ct>
      <x:if test="@IsComment = 'false'">
        <x:choose>
          <x:when test="@IsAmountAbsolute = 'true'">
            <x:value-of select="iiko:Format($Res/@KitchenModifierAbsoluteAmountPattern, @Amount)" />
          </x:when>
          <x:otherwise>
            <x:value-of select="iiko:Format($Res/@KitchenModifierAmountPattern, @Amount)" />
          </x:otherwise>
        </x:choose>
      </x:if>
    </ct>
  </x:template>

  <!-- Products list -->
  <x:template match="Products">
    <x:for-each select="Product">
      <x:if test="position() > 1">
        <x:call-template name="MixRow" />
      </x:if>
      <x:apply-templates select="." />
      <x:apply-templates select="Modifiers" />
      <x:if test="@ParentProduct">
        <x:value-of select="iiko:Format($Res/@ParentProductForModifierPattern, @ParentProduct)" />
      </x:if>
      <x:if test="@GuestPlace > 0">
        <x:call-template name="GuestPlaceTable" />
      </x:if>
      <x:if test="@BarCode">
        <center>
          <barcode>
            <x:value-of select="@BarCode" />
          </barcode>
        </center>
      </x:if>
    </x:for-each>
  </x:template>

  <x:template match="Product">
    <f1>
      <table>
        <x:call-template name="OrderItemTableColumns" />
        <cells>
          <x:choose>
            <x:when test="@CookingPlaceName">
              <ct font="f0">
                <x:value-of select="@Code" />
              </ct>
            </x:when>
            <x:otherwise>
              <ct>
                <x:value-of select="@Code" />
              </ct>
            </x:otherwise>
          </x:choose>
          <c>
            <x:choose>
              <x:when test="@CookingPlaceName">
                  <x:value-of select="iiko:Format($Res/@CookingPlaceTemplate, @CookingPlaceName, @Name)" />
              </x:when>
              <x:otherwise>
                <x:value-of select="@Name" />
              </x:otherwise>
            </x:choose>
          </c>
          <ct>
            <x:value-of select="iiko:FormatAmount(@Amount)" />
          </ct>
        </cells>
      </table>
    </f1>
  </x:template>

  <!-- Header -->
  <x:template name="HeaderCommon">
    <x:value-of select="iiko:Format($Res/@KitchenHeaderPattern, iiko:FormatFullDateTime(Settings/@Now), Info/Order/@Number, Info/@PrinterCounter, Info/Order/@WaiterName)" />
    <np />
    <np />
    <x:call-template name="HeaderTableCommon" />
  </x:template>

  <x:template name="HeaderTableCommon">
    <f1>
      <split>
        <x:value-of select="iiko:Format($Res/@ServiceChequeHeaderLocationPattern, Info/Order/@TableNumber, Info/Order/@SectionName, Info/Order/@GuestsCount)" />
      </split>
    </f1>
    <np />
    <np />
  </x:template>

  <!-- Footer -->
  <x:template name="FooterCommon">
    <x:if test="@IsRepeatedPrint = 'true'">
      <np />
      <f2>
        <x:value-of select="$Res/@KitchenRepeated" />
      </f2>
      <np />
    </x:if>
    <np />
    <np />
  </x:template>

  <!-- Other named templates -->
  <x:template name="MixRow">
    <x:call-template name="CenterTextRow">
      <x:with-param name="cells" select="$Res/@Mix" />
    </x:call-template>
  </x:template>

  <x:template name="PartOfMixRow">
    <x:call-template name="CenterTextRow">
      <x:with-param name="cells" select="$Res/@PartOfMixText" />
    </x:call-template>
  </x:template>

  <x:template name="CenterTextRow">
    <x:param name="cells" />
    <table>
      <x:call-template name="OrderItemTableColumns" />
      <cells>
        <c />
        <c>
          <x:value-of select="$cells" />
        </c>
        <c />
      </cells>
    </table>
  </x:template>

  <x:template name="OrderItemTableColumns">
    <columns>
      <column autowidth="" minwidth="4" />
      <column formatter="split" />
      <column autowidth="" />
    </columns>
  </x:template>

  <x:template name="GuestPlaceTable">
    <pair fit="right" left="{$Res/@GuestPlaceTitle}" right="{@GuestPlace}" />
  </x:template>

</x:stylesheet>
