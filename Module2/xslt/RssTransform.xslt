<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:b="http://library.by/catalog"
                exclude-result-prefixes="msxsl">

  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="b:catalog">
    <xsl:element name="rss">
      <xsl:attribute name = "version">2.0</xsl:attribute>
      <xsl:element name="channel">
        <xsl:apply-templates select="b:catalog"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="b:book">
    <xsl:element name="item">
      <xsl:element name="title">
        <xsl:value-of select="b:title"/>
      </xsl:element>

      <xsl:variable name="isbn" select="b:isbn"/>
      <xsl:variable name="genre" select="b:genre"/>
      <xsl:if test="$isbn != '' and $genre = 'Computer'">
        <xsl:element name="link">
          <xsl:value-of select="concat('http://my.safaribooksonline.com/', $isbn , '/')" />
        </xsl:element>
      </xsl:if>

      <xsl:element name="updated">
        <xsl:value-of select="b:registration_date"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="node() | @*" />

</xsl:stylesheet>