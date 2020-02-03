local LBF = LibStub("LibButtonFacade", true)
local L = LibStub("AceLocale-3.0"):GetLocale("Ovale")

local function Update(self, element, minAttente, actionTexture, actionInRange, actionCooldownStart, actionCooldownDuration,
				actionUsable, actionShortcut, actionIsCurrent, actionEnable, spellName, actionTarget)

	if (minAttente~=nil and actionTexture) then


		if (actionTexture~=self.actionCourante or self.ancienneAttente==nil or
			(minAttente~=Ovale.maintenant and minAttente>self.ancienneAttente+0.01) or
			(minAttente < self.finAction-0.01)) then
			if (actionTexture~=self.actionCourante or self.ancienneAttente==nil or
					(minAttente~=Ovale.maintenant and minAttente>self.ancienneAttente+0.01)) then
				self.debutAction = Ovale.maintenant
			end
			self.actionCourante = actionTexture
			self.finAction = minAttente
			if (minAttente == Ovale.maintenant) then
				self.cd:Hide()
			else
				self.lastSound = nil
				self.cd:Show()

				-- if (actionTexture == "Interface\\Icons\\Spell_Shadow_ShadowBolt"
				--   or actionTexture == "Interface\\Icons\\Spell_Shadow_ChillTouch"
				-- 	or actionTexture == "Interface\\Icons\\Spell_Shadow_haunting"
				-- 	or actionTexture == "Interface\\Icons\\Spell_Shadow_Unstableaffliction"
				-- )
				-- 	then
        --   SendChatMessage(actionTexture,"SAY")
				-- end

				self.cd:SetCooldown(self.debutAction, self.finAction - self.debutAction);
			end
		end

		self.ancienneAttente = minAttente

		-- L'icône avec le cooldown
		self.icone:Show()
		self.icone:SetTexture(actionTexture);

		local num = self.shortcut:GetText()
		if "1" == num --and start~=nil and actionTexture
			or "2" == num --and start~=nil and actionTexture
			or "3" == num --and start~=nil and actionTexture
			or "4" == num --and start~=nil and actionTexture
			or "5" == num --and start~=nil and actionTexture
			or "6" == num --and start~=nil and actionTexturez
		then
			-- print(actionTexture.." - "..num.."5 -- "..spellName)
			changeColor(self)
		end

		if (actionUsable) then
			self.icone:SetAlpha(1.0)
		else
			self.icone:SetAlpha(0.33)
		end

		local red
		if (minAttente > actionCooldownStart + actionCooldownDuration + 0.01 and minAttente > Ovale.maintenant
			and minAttente>Ovale.attenteFinCast) then
			self.icone:SetVertexColor(0.75,0.2,0.2)
			red = true
		else
			self.icone:SetVertexColor(1,1,1)
		end

		if (minAttente==Ovale.maintenant) then
			self.cd:Hide()
		end

		if element.params.sound and not self.lastSound then
			local delay = self.soundtime or 0.5
			if Ovale.maintenant>=minAttente - delay then
				self.lastSound = element.params.sound
				-- print("Play" .. self.lastSound)
				PlaySoundFile(self.lastSound)
			end
		end

		-- La latence
		if minAttente>Ovale.maintenant and Ovale.db.profile.apparence.highlightIcon and not red then
			local lag = 0.6
			local newShouldClick
			if minAttente<Ovale.maintenant + lag then
				newShouldClick = true
			else
				newShouldClick = false
			end
			if self.shouldClick ~= newShouldClick then
				if newShouldClick then
					self:SetChecked(1)
				else
					self:SetChecked(0)
				end
				self.shouldClick = newShouldClick
			end
		elseif self.shouldClick then
			self.shouldClick = false
			self:SetChecked(0)
		end

		-- Le temps restant
		if (Ovale.db.profile.apparence.numeric) then
			self.remains:SetText(string.format("%.1f", minAttente - Ovale.maintenant))
			self.remains:Show()
		else
			self.remains:Hide()
		end

		-- Le raccourcis clavier
		if (Ovale.db.profile.apparence.raccourcis) then
			self.shortcut:Show()
			self.shortcut:SetText(actionShortcut)
		else
			self.shortcut:Hide()
			self.shortcut:SetText("")
		end

		-- L'indicateur de portée
		self.aPortee:Show()
		if (actionInRange==1) then
			self.aPortee:SetVertexColor(0.6,0.6,0.6)
			self.aPortee:Show()
		elseif (actionInRange==0) then
			self.aPortee:SetVertexColor(1.0,0.1,0.1)
			self.aPortee:Show()
		else
			self.aPortee:Hide()
		end
		if actionTarget=="focus" then
			self.focusText:Show()
		else
			self.focusText:Hide()
		end
		self:Show()
	else
		self.icone:Hide()
		self.aPortee:Hide()
		self.shortcut:Hide()
		self.remains:Hide()
		self.focusText:Hide()
		if Ovale.db.profile.apparence.hideEmpty then
			self:Hide()
		else
			self:Show()
		end
		if self.shouldClick then
			self:SetChecked(0)
			self.shouldClick = false
		end
	end


	return minAttente,element
end

local function SetSkinGroup(self, _skinGroup)
	self.skinGroup = _skinGroup
	self.skinGroup:AddButton(self)
end

local function SetSize(self, width, height)
	self:SetWidth(width)
	self:SetHeight(height)
	if (not LBF) then
		self.normalTexture:SetWidth(width*66/36)
		self.normalTexture:SetHeight(height*66/36)
		self.shortcut:SetWidth(width)
		self.remains:SetWidth(width)
	end
end

local function SetHelp(self, help)
	self.help = help
end

local HelloWorld1 = CreateFrame("Frame", nil, UIParent)
HelloWorld1:SetFrameStrata("BACKGROUND")
HelloWorld1:SetWidth(25)
HelloWorld1:SetHeight(25)
HelloWorld1.texture = HelloWorld1:CreateTexture(nil,"BACKGROUND")
HelloWorld1.texture:SetAllPoints(HelloWorld1)
HelloWorld1:SetPoint("TOPLEFT",0,0)
HelloWorld1:Show()
-- 
-- function changeColor(self)
-- 	local r, g, b
--   local num = self.shortcut:GetText()
-- 	if "1" == num then
-- 		r = 1
-- 		g = 0.5
-- 		b = 0.5
-- 	elseif "2" == num then
-- 		r = 0.5
-- 		g = 1
-- 		b = 0.5
-- 	elseif "3" == num then
-- 		r = 0.5
-- 		g = 0.5
-- 		b = 1
-- 	elseif "4" == num then
-- 		r = 1
-- 		g = 1
-- 		b = 0.5
-- 	elseif "5" == num then
-- 		r = 0.5
-- 		g = 1
-- 		b = 1
-- 	elseif "6" == num then
-- 		r = 1
-- 		g = 0.5
-- 		b = 1
-- 	end
--
-- 	-- SendChatMessage(num.." - "..r..":"..g..":"..b,"SAY")
--
-- 	HelloWorld1.texture:SetTexture(r, g, b)
-- end

function OvaleIcone_OnClick(self)
changeColor(self)
	 Ovale:ToggleOptions()
	self:SetChecked(0)
end

function OvaleIcone_OnEnter(self)
	-- SendChatMessage("Hello2 ","SAY")
	if self.help or next(Ovale.casesACocher) or next(Ovale.listes) then
		GameTooltip:SetOwner(self, "ANCHOR_BOTTOMLEFT")
		if self.help then
			GameTooltip:SetText(L[self.help])
		end
		if next(Ovale.casesACocher) or next(Ovale.listes) then
			GameTooltip:AddLine(L["Cliquer pour afficher/cacher les options"],1,1,1)
		end
		GameTooltip:Show()
	end
end

function OvaleIcone_OnLeave(self)
	if self.help  or next(Ovale.casesACocher) or next(Ovale.listes)  then
		GameTooltip:Hide()
	end
end

function OvaleIcone_OnLoad(self)
	local name = self:GetName()
	self.icone = _G[name.."Icon"]
	self.shortcut = _G[name.."HotKey"]
	self.remains = _G[name.."Name"]
	self.aPortee = _G[name.."Count"]
	self.aPortee:SetText(RANGE_INDICATOR)
	self.cd = _G[name.."Cooldown"]
	self.normalTexture = _G[name.."NormalTexture"]

	self.focusText = self:CreateFontString(nil, "OVERLAY");
	self.focusText:SetFontObject("GameFontNormal");
	self.focusText:SetAllPoints(self);
	self.focusText:SetTextColor(1,1,1);
	self.focusText:SetText(L["Focus"])

	self:RegisterForClicks("LeftButtonUp")
	self.SetSkinGroup = SetSkinGroup
	self.Update = Update
	self.SetSize = SetSize
	self.SetHelp = SetHelp
end
