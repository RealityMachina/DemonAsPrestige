namespace DemonAsPrestige.Config {
    public interface IUpdatableSettings {
        void OverrideSettings(IUpdatableSettings userSettings);
    }
}
