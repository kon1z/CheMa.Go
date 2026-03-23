using CheMa.Go.Domain.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace CheMa.Go.Applications.Dtos
{
    public class PassengerDto : EntityDto<long>
    {
        /// <summary>
        /// �˿ͽ���״̬
        /// </summary>
        public PassengerStatus Status { get; set; }
        /// <summary>
        /// �˿ͳƺ�
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// �˿���ϵ��ʽ
        /// </summary>
        public string Phone { get; set; } = null!;
        /// <summary>
        /// �˿�����
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// ԤԼʱ��
        /// </summary>
        public DateTime AppointmentTime { get; set; }
        /// <summary>
        /// ��������Id
        /// </summary>
        public long? OrderId { get; set; }
        /// <summary>
        /// �����Ƶ�Id
        /// </summary>
        public long HotelId { get; set; }

        /// <summary>
        /// �����Ƶ�
        /// </summary>
        public HotelDto? Hotel { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        public string Remark { get; set; } = string.Empty;

        public LocationInfoDto Location { get; set; } = new();
    }
}

