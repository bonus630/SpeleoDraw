<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:frmwrk="Corel Framework Data">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <frmwrk:uiconfig>
   
    <frmwrk:applicationInfo userConfiguration="true" />
  </frmwrk:uiconfig>

  <!-- Copy everything -->
  <xsl:template match="node()|@*">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="uiConfig/items">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>
		<!-- Define the button will contains menu is same in all projects -->
		<itemData guid="f1d3d1d0-cc8d-4f04-91cb-7112255b8af1" noBmpOnMenu="true"
				  type="flyout"
				  dynamicCategory="2cc24a3e-fe24-4708-9a74-9c75406eebcd"
				  userCaption="Bonus630 Dockers"
				  enable="true"
				  flyoutBarRef="FB727225-CEA7-4D27-BB27-52C687B53029"
                />
      <!-- Define the button which shows the docker -->
      <itemData guid="2d7c6524-77e7-4805-9cbe-ef5e5474e43c" noBmpOnMenu="true"
                type="checkButton"
                check="*Docker('502a4e18-1494-4ca8-934d-89f823805147')"
                dynamicCategory="2cc24a3e-fe24-4708-9a74-9c75406eebcd"
                userCaption="SpeleoDraw"
                enable="true"/>

      <!-- Define the web control which will be placed on our docker -->
      <itemData guid="5f5a1964-5a20-4295-a767-a661cd4f8931"
                type="wpfhost"
                hostedType="Addons\SpeleoDraw\SpeleoDraw.dll,br.corp.bonus630.VSTA.SpeleoDraw.DockerUI"
                enable="true"/>

    </xsl:copy>
  </xsl:template>
	<!-- Define the new menu is same in all others project-->
	<xsl:template match="uiConfig/commandBars">
		<xsl:copy>
			<xsl:apply-templates select="node()|@*"/>

			<commandBarData guid="FB727225-CEA7-4D27-BB27-52C687B53029"
							type="menu"
							nonLocalizableName="Bonus630 Dockers"
							flyout="true">
				<menu>

					<!--Here change to new item-->
					<!--<item guidRef="DF67BEBE-6551-4F3B-BE5B-1BF46E16AB67"/>-->

				</menu>
			</commandBarData>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="uiConfig/commandBars/commandBarData[guid='FB727225-CEA7-4D27-BB27-52C687B53029']/menu">
		<xsl:copy>
			<xsl:apply-templates select="node()|@*"/>

					<!--Here change to new item-->
					<item guidRef="2d7c6524-77e7-4805-9cbe-ef5e5474e43c"/>

		</xsl:copy>
	</xsl:template>
  <xsl:template match="uiConfig/dockers">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>

      <!-- Define the web docker -->
      <dockerData guid="502a4e18-1494-4ca8-934d-89f823805147"
                  userCaption="SpeleoDraw"
                  wantReturn="true"
                  focusStyle="noThrow">
        <container>
          <!-- add the webpage control to the docker -->
          <item dock="fill" margin="0,0,0,0" guidRef="5f5a1964-5a20-4295-a767-a661cd4f8931"/>
        </container>
      </dockerData>
    </xsl:copy>
  </xsl:template>

</xsl:stylesheet>
