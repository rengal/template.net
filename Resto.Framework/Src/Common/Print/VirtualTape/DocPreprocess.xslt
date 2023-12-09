<?xml version="1.0" encoding="utf-8"?>
<x:stylesheet version="1.0" xmlns:x="http://www.w3.org/1999/XSL/Transform">
  <x:output method="xml" indent="yes"/>

  <x:template match="@*|node()">
    <x:copy>
      <x:apply-templates select="@*|node()"/>
    </x:copy>
  </x:template>

  <x:template match="line">
    <x:choose>
      <x:when test="@symbols">
        <fill symbols="{@symbols}">
          <np />
        </fill>
      </x:when>
      <x:otherwise>
        <fill symbols="-">
          <np />
        </fill>
      </x:otherwise>
    </x:choose>
  </x:template>

  <x:template match="linecell">
    <c colspan="0">
      <x:choose>
        <x:when test="@symbols">
          <fill symbols="{@symbols}">
            <np />
          </fill>
        </x:when>
        <x:otherwise>
          <fill symbols="-">
            <np />
          </fill>
        </x:otherwise>
      </x:choose>
    </c>
  </x:template>

  <x:template match="pair">
    <table>
      <columns>
        <x:choose>
          <x:when test="@fit='left'">
            <column align="left" autowidth="" />
            <column align="right" />
          </x:when>
          <x:otherwise>
            <column align="left" />
            <column align="right" autowidth="" />
          </x:otherwise>
        </x:choose>
      </columns>
      <cells>
        <ct>
          <x:value-of select="@left" />
        </ct>
        <ct>
          <x:value-of select="@right" />
        </ct>
      </cells>
    </table>
  </x:template>

  <x:template match="leftpair">
    <table>
      <columns>
        <column align="left" autowidth="" />
        <column align="left" />
      </columns>
      <cells>
        <ct>
          <x:value-of select="@left" />
        </ct>
        <ct>
          <x:value-of select="@right" />
        </ct>
      </cells>
    </table>
  </x:template>

</x:stylesheet>
