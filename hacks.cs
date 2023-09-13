using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Mem;
using System.Windows.Forms;
using System.Threading;

namespace our_first_menu
{
    public class hacks
    {
        public Memory mem = new Memory();
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);

        public hacks()  
        {
            mem.GetProcess("csgo");

            globals.engineAdress = mem.GetModuleBase("engine.dll");
            globals.clientAdress = mem.GetModuleBase("client.dll");
        }
        public void TriggerBot()
        {
            while (true)
            {
                Thread.Sleep(1);

                if (!globals.triggerBot)
                    continue;

                while (GetAsyncKeyState(Keys.XButton1) < 0)
                {
                    // get local player
                    var localPlayer = mem.ReadPointer(globals.clientAdress, offsets.dwLocalPlayer);
                    var localTeam = BitConverter.ToInt32(mem.ReadBytes(localPlayer, offsets.m_iTeamNum, 4), 0);

                    var crosshairID = BitConverter.ToInt32(mem.ReadBytes(localPlayer, offsets.m_iCrosshairId, 4), 0);

                    // if crosshair id dont exist or insnt looking at player
                    if (crosshairID > 64 || crosshairID == 0)
                        continue;

                    var player = mem.ReadPointer(globals.clientAdress, offsets.dwEntityList + (crosshairID - 1) * 0x10);
                    var team = BitConverter.ToInt32(mem.ReadBytes(player, offsets.m_iTeamNum, 4), 0);

                    if (localTeam == team)
                        continue;

                    Thread.Sleep(globals.triggerBotDelay);
                    mem.WriteBytes(globals.clientAdress, offsets.dwForceAttack, BitConverter.GetBytes(6));
                    Thread.Sleep(40);
                    mem.WriteBytes(globals.clientAdress, offsets.dwForceAttack, BitConverter.GetBytes(4));
                }
            }
        }
        public void Glow()
        {
            while (true)
            {
                if (!globals.glow)
                    continue;

                Thread.Sleep(10);
                    
                var localPlayer = mem.ReadPointer(globals.clientAdress, offsets.dwLocalPlayer);

                if (localPlayer == null)
                    continue;

                var glowManager = mem.ReadPointer(globals.clientAdress, offsets.dwGlowObjectManager);

                if (glowManager == null)
                    continue;

                var localTeam = BitConverter.ToInt32(mem.ReadBytes(localPlayer, offsets.m_iTeamNum, 4), 0);

                // loop trough entity list
                for (int i = 1; i <= 32; i++)
                {
                    var player = mem.ReadPointer(globals.clientAdress, offsets.dwEntityList + i * 0x10);

                    if (player == null)
                        continue;
                                                                                                        
                    var team = BitConverter.ToInt32(mem.ReadBytes(player, offsets.m_iTeamNum, 4), 0);

                    // only enemies
                    if (localTeam == team)
                        continue;

                    var lifeState = BitConverter.ToInt32(mem.ReadBytes(player, offsets.m_lifeState, 4), 0);

                    // if dead continue
                    if (lifeState != 0)
                        continue;

                    var glowIndex = BitConverter.ToInt32(mem.ReadBytes(player, offsets.m_iGlowIndex, 4), 0);
                        
                    // making them glow :)
                    mem.WriteBytes(glowManager, (glowIndex * 0x38) + 0x8, BitConverter.GetBytes(1f)); // R
                    mem.WriteBytes(glowManager, (glowIndex * 0x38) + 0xC, BitConverter.GetBytes(0f)); // G
                    mem.WriteBytes(glowManager, (glowIndex * 0x38) + 0x10, BitConverter.GetBytes(0f)); // B
                    mem.WriteBytes(glowManager, (glowIndex * 0x38) + 0x14, BitConverter.GetBytes(1f)); // alfa

                    mem.WriteBytes(glowManager, (glowIndex * 0x38) + 0x28, BitConverter.GetBytes(true));
                    mem.WriteBytes(glowManager, (glowIndex * 0x38) + 0x29, BitConverter.GetBytes(false));
                }
            }
        }
        public void Aimbot()
        {
            while (true)
            {
                Thread.Sleep(1);

                if (!globals.aimbot)
                    continue;

                while(GetAsyncKeyState(Keys.XButton2) < 0)
                {
                    var localPlayer = mem.ReadPointer(globals.clientAdress, offsets.dwLocalPlayer);
                    var localTeam = BitConverter.ToInt32(mem.ReadBytes(localPlayer, offsets.m_iTeamNum, 4), 0);

                    byte[] localOriginBytes = mem.ReadBytes(localPlayer, offsets.m_vecOrigin, 12);
                    Vector3 localOrigin = new Vector3(
                            BitConverter.ToSingle(localOriginBytes, 0),
                            BitConverter.ToSingle(localOriginBytes, 4),
                            BitConverter.ToSingle(localOriginBytes,8)
                        );

                    byte[] localVecOffsetBytes = mem.ReadBytes(localPlayer, offsets.m_vecViewOffset, 12);
                    Vector3 localVecOffset = new Vector3(
                            BitConverter.ToSingle(localVecOffsetBytes, 0),
                            BitConverter.ToSingle(localVecOffsetBytes, 4),
                            BitConverter.ToSingle(localVecOffsetBytes, 8)
                        );

                    Vector3 localEyePosition = localOrigin + localVecOffset;

                    var clientState = mem.ReadPointer(globals.engineAdress, offsets.dwClientState);

                    byte[] viewAnglesBytes = mem.ReadBytes(clientState, offsets.dwClientState_ViewAngles, 12);
                    Vector3 viewAngles = new Vector3(
                            BitConverter.ToSingle(viewAnglesBytes, 0),
                            BitConverter.ToSingle(viewAnglesBytes, 4),
                            BitConverter.ToSingle(viewAnglesBytes, 8)
                        );

                    byte[] aimPunchBytes = mem.ReadBytes(localPlayer, offsets.m_aimPunchAngle, 12);
                    Vector3 aimPunch = new Vector3(
                            BitConverter.ToSingle(aimPunchBytes, 0),
                            BitConverter.ToSingle(aimPunchBytes, 4),
                            BitConverter.ToSingle(aimPunchBytes, 8)
                        );

                    var bestFOV = globals.aimbotFOV;
                    var bestAngle = new Vector3();

                    for (int i = 1; i < 32; i++)
                    {
                        var player = mem.ReadPointer(globals.clientAdress, offsets.dwEntityList + i * 0x10);

                        var team = BitConverter.ToInt32(mem.ReadBytes(player, offsets.m_iTeamNum, 4), 0);
                        var dormant = BitConverter.ToBoolean(mem.ReadBytes(player, offsets.m_bDormant, 2), 0);
                        var playerHealth = BitConverter.ToInt32(mem.ReadBytes(player, offsets.m_iHealth, 4), 0);

                        if (localTeam == team || dormant == true || playerHealth < 1)
                            continue;

                        if (BitConverter.ToInt32(mem.ReadBytes(player, offsets.m_bSpottedByMask, 4), 0) == 0)
                            continue;

                        var boneMatrix = mem.ReadPointer(player, offsets.m_dwBoneMatrix);

                        var playerHeadPosition = new Vector3(
                            BitConverter.ToSingle(mem.ReadBytes(boneMatrix + 0x30 * 8 + 0x0C, 4), 0),
                            BitConverter.ToSingle(mem.ReadBytes(boneMatrix + 0x30 * 8 + 0x1C, 4), 0),
                            BitConverter.ToSingle(mem.ReadBytes(boneMatrix + 0x30 * 8 + 0x2C, 4), 0)
                        );

                        var angle = Vector3.CalculateAngle(
                                localEyePosition,
                                playerHeadPosition,
                                viewAngles + aimPunch
                            );

                        var fov = Vector3.Hypot(angle.x, angle.y);

                        if(fov < bestFOV)
                        {
                            bestAngle = angle;
                            bestFOV = fov;
                        }
                    }

                    if (!bestAngle.IsZero())
                    {
                        var help = viewAngles + bestAngle / (globals.aimbotSmooth * 2);

                        mem.WriteBytes(clientState, offsets.dwClientState_ViewAngles, BitConverter.GetBytes(help.x));
                        mem.WriteBytes(clientState, offsets.dwClientState_ViewAngles + 0x4, BitConverter.GetBytes(help.y));
                    }
                }
            }
        }
    }
}
