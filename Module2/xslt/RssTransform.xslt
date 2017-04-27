<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:library="http://library.by/catalog"
                exclude-result-prefixes="msxsl">

  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="library:catalog">
    <xsl:element name="rss">
      <xsl:attribute name = "version">2.0</xsl:attribute>
      <xsl:element name="channel">
        <xsl:element name="title">Library.by</xsl:element>
        <xsl:element name="description">Library</xsl:element>
        <xsl:apply-templates/>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="library:book">
    <xsl:element name="item">
      <xsl:element name="title">
        <xsl:value-of select="library:title"/>
      </xsl:element>

      <xsl:variable name="isbn" select="library:isbn"/>
      <xsl:variable name="genre" select="library:genre"/>
      <xsl:if test="$isbn != '' and $genre = 'Computer'">
        <xsl:element name="link">
          <xsl:value-of select="concat('http://my.safaribooksonline.com/', $isbn , '/')" />
        </xsl:element>
      </xsl:if>

      <xsl:element name="updated">
        <xsl:value-of select="library:registration_date"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="node() | @*" />
  
</xsl:stylesheet>