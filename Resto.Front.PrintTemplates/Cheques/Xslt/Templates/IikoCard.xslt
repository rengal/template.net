<?xml version="1.0" encoding="utf-8"?>
<x:stylesheet version="1.0" xmlns:x="http://www.w3.org/1999/XSL/Transform">
  <x:output method="xml" version="1.0" encoding="utf-8" indent="yes" />

  <x:variable name="Res" select="/*/Settings/Resources" />

  <!-- Чек: ошибка, неподдерживаемый корень XML -->
  <x:template match="/*">
    <doc>
      <f2>ОШИБКА</f2>
      <f1>
        Неподдерживаемый тип чеков: <x:value-of select="name(/*)" />
      </f1>
    </doc>
  </x:template>

  <!-- iikoCard -->
  <x:template match="/IikoCard">
    <doc>
      <left>
        <split>
          <x:value-of select="Settings/@ChequeHeader" />
        </split>
      </left>
      <np/>
      <left>
        <split>
          <x:value-of select="iiko:FormatFullDateTime(Settings/@Now)" />
        </split>
      </left>
      <center>
        <x:value-of select="CardInfo/@OperationName"/>
      </center>
      <np/>
      <x:if test="@HasOrder = 'true'">
        <pair fit="right" left="{iiko:Format($Res/@BillHeaderSectionPattern, Order/@SectionName)}" right="{iiko:Format($Res/@BillHeaderTablePattern, Order/@TableNumber)}" />
        <pair fit="right" left="{iiko:Format($Res/@BillHeaderOrderOpenPattern, iiko:FormatLongDateTime(Order/@OrderOpenTime))}" right="{iiko:Format($Res/@BillHeaderOrderNumberPattern, Order/@Number)}" />
        <left>
          <x:value-of select="iiko:Format($Res/@BillHeaderWaiterPattern, Order/@WaiterName)" />
        </left>
        <np/>
      </x:if>
      <!-- Header (end) -->
      <left>
        <x:value-of select="iiko:Format($Res/@BillHeaderCashierPattern, CardInfo/@CashierName)" />
      </left>
      <table>
        <columns>
          <column autowidth="" />
          <column align="right" />
        </columns>
        <cells>
          <ct>
            <x:value-of select="$Res/@Terminal" />
          </ct>
          <c>
            <split>
              <x:value-of select="CardInfo/@Terminal" />
            </split>
          </c>
        </cells>
      </table>

      <x:choose>
        <x:when test="@IsSuccessful = 'true'">
          <table>
            <columns>
              <column autowidth="" />
              <column align="right" />
            </columns>
            <cells>

              <ct>
                <x:value-of select="$Res/@IikoCardCardNumber" />
              </ct>
              <ct>
                <x:value-of select="/*/CardInfo/@CardNumber" />
              </ct>

              <ct>
                <x:value-of select="$Res/@IikoCardCardOwner" />
              </ct>
              <c>
                <split>
                  <x:value-of select="/*/CardInfo/@CardOwner" />
                </split>
              </c>

              <ct>
                <x:value-of select="$Res/@IikoCardRowSum" />
              </ct>
              <ct>
                <x:value-of select="iiko:FormatMoney(/*/CardInfo/@Amount)" />
              </ct>
            </cells>
          </table>
          <table>
            <columns>
              <column />
              <column autowidth="" align="right" />
            </columns>
            <cells>
              <x:for-each select="/*/CardInfo/ChequeRows/IikoCardRow">
                <ct>
                  <x:value-of select="@Name" />
                </ct>
                <ct>
                  <x:value-of select="iiko:FormatMoney(@Value)" />
                </ct>
                <x:if test="@HasLines = 'true'" >
                  <c colspan="2">
                    <split>
                      <x:value-of select="@Lines" />
                    </split>
                  </c>
                </x:if>
              </x:for-each>
            </cells>
          </table>          
          <table>
            <columns>
              <column autowidth="" />
              <column align="right" />
            </columns>
            <cells>

              <x:if test="/*/CardInfo/@IsPayment = 'false'">
                <x:if test="/*/CardInfo/@HasOperationIds = 'true'">
                  <ct>
                    <x:value-of select="/*/CardInfo/@OperationNumTitle" />
                  </ct>
                  <ct>
                    <x:value-of select="/*/CardInfo/@OperationId" />
                  </ct>

                  <ct>
                    <x:value-of select="/*/CardInfo/@RequestNumTitle" />
                  </ct>
                  <ct>
                    <x:value-of select="/*/CardInfo/@RequestId" />
                  </ct>
                </x:if>
              </x:if>

              <!-- Дата хоста-->
              <x:if test="/*/CardInfo/@HasHostTime = 'true'">
                <ct>
                  <x:value-of select="$Res/@IikoCardFooterOperationDate" />
                </ct>
                <ct>
                  <x:value-of select="iiko:FormatFullDateTime(/*/CardInfo/@HostTime)" />
                </ct>
              </x:if>
            </cells>
          </table>

        </x:when>
        <x:otherwise>
          <split>
            <x:value-of select="CardInfo/@ErrorString" />
          </split>
        </x:otherwise>
      </x:choose>
      <np/>
      <np/>
      <center>
        <x:choose>
          <x:when  test="@IsSuccessful = 'true'">
            <x:value-of select="$Res/@IikoCardOperationSuccessful" />
          </x:when>
          <x:otherwise>
            <x:value-of select="$Res/@IikoCardOperationFailed" />
          </x:otherwise>
        </x:choose>

      </center>
      <!-- Body (end) -->

      <!-- Footer (begin) -->
      <np />
      <center>
        <split>
          <x:value-of select="Settings/@ChequeFooter" />
        </split>
      </center>
      <np />
      <np />
      <x:if test="CardInfo/@HasSignature = 'true'">
        <np />
        <line />
        <center>
          <x:value-of select="$Res/@Signature"/>
        </center>
      </x:if>

      <np />
      <!-- Footer (end) -->

    </doc>
  </x:template>

  <x:template match="/IikoCardShort">
    <doc>
      <left>
        <wrap>
          <x:value-of select="$Res/@IikoCardCardNumber" />
          <x:value-of select="/*/CardInfo/@CardNumber" />
        </wrap>
      </left>

      <left>
        <wrap>
          <x:value-of select="$Res/@IikoCardCardOwner" />
          <x:value-of select="/*/CardInfo/@CardOwner" />
        </wrap>
      </left>

      <left>
        <wrap>
          <x:value-of select="$Res/@IikoCardRowSum" />
          <x:value-of select="iiko:FormatMoney(/*/CardInfo/@Amount)" />
        </wrap>
      </left>

      <x:for-each select="/*/CardInfo/ChequeRows/IikoCardRow">
        <left>
          <wrap>
            <x:value-of select="@Name" />

            <x:value-of select="iiko:FormatMoney(@Value)" />
          </wrap>
        </left>
        <x:if test="@HasLines = 'true'" >
          <left>
            <wrap>
              <x:value-of select="@Lines" />
            </wrap>
          </left>
        </x:if>
      </x:for-each>

      <!-- Дата хоста-->
      <x:if test="/*/CardInfo/@HasHostTime = 'true'">
        <left>
          <wrap>
            <x:value-of select="$Res/@IikoCardFooterOperationDate" />
            <x:value-of select="iiko:FormatFullDateTime(/*/CardInfo/@HostTime)" />
          </wrap>
        </left>
      </x:if>
    </doc>
  </x:template>

</x:stylesheet>
