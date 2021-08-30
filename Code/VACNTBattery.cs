using Game.Constants;
using Game.Data;
using Game.Systems.Energy;
using KL.I18N;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Components
{
  public sealed class VACNTBatteryComp : BaseComponent<BatteryComp>, IUIDataProvider, IEnergyProducer
  {
    private UDB stateBlock;
    private UDB chargeBlock;
    private UDB dischargeBlock;
    private float unproducedPct;
    private float unproducedAmount;
    private BatteryComp.Status status;
    private float charge;
    private float chargeBefore;
    private float chargeRate;
    private int capacity;
    private float maxChargeRate;
    private float maxDischargeRate;
    private float currentRate;
    private UDB dataBlock;
    private EventNotification dischargeEvent;
    private float produced;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Register() => BaseComponent<BatteryComp>.AddComponentPrototype((IComponent) new VACNTBatteryComp());

    protected override void OnConfig()
    {
      this.capacity = this.Config.GetInt("Capacity", 30000f);
      this.maxChargeRate = this.Config.GetFloat("ChargeRate", 300f);
      this.maxDischargeRate = this.Config.GetFloat("DischargeRate", 100f);
    }

    public override string ToString() => string.Format(" * Battery [Cap: {0:0.0}/{1}, Rate: {2:0.0} (-{3}/{4})]", (object) this.charge, (object) this.capacity, (object) this.currentRate, (object) this.maxChargeRate, (object) this.maxDischargeRate);

    protected override void OnLoad(ComponentData data)
    {
      this.charge = data.GetFloat("Charge", 0.0f);
      this.charge = Mathf.Clamp(this.charge, 0.0f, (float) this.capacity);
    }

    public override void OnLateReady(bool wasLoaded) => this.AddCharge(0.0f);

    public override void OnSave()
    {
      double num = (double) this.GetOrCreateData().SetFloat("Charge", this.charge);
    }
  }
}
