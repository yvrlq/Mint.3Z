using System.ComponentModel;

namespace Library.SystemModels
{
	public enum MiniGames
	{
        [Description("����С��Ϸ")]
        [Category("���������Ķӡ���һ����ȡ�����Է������ͻ��콱̨�Ķ����ʤ.")]
        CaptureTheFlag,
        [Description("������")]
        [Category("ÿ���˶����ڵ���������ȥ��Ҫô�����һ����������ң�Ҫô�����һ�������ŵ���ң��Ա��ܹ��ҵ����صı���.")]
        TreasureIsland,
        [Description("�ִ�����")]
        [Category("ɱ��������ٶȱ���Ķ��ֿ졣ÿ����ɱ��һ������������ڶԷ��Ķ�������������Ϊ���һ֧Ӯ�ñ����Ķ���.")]
        MonsterTennis,
        [Description("���ҵ�ս��")]
        [Category("ÿ����Ҷ���Ϊ���Լ������3��վ�ŵ���ҽ����ǰ3��ɱ�ֵĽ���.")]
        BattleRoyal,
        [Description("�ŶӼ�ս")]
        [Category("���Ӿ�һ���ۡ�Ϊ�������׬Ǯ��ɱ�ˡ����վ�ŵĶ�����Ӯ�ң�ÿ�������ǰ����ɱ�ֶ��н�����.")]
        TeamBattleRoyal
    }
}
