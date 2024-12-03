using AutoMapper;
using Doantour.DTO;
using Doantour.Helpers.ContentEmail;
using Doantour.Repository;
using Doantour.Respository;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

public class SendMailService
{
    private readonly string _smtpServer = "smtp.gmail.com";
    private readonly int _smtpPort = 587;
    private readonly string _smtpUser = "hachutraveltour@gmail.com";
    private readonly string _smtpPass = "dqkn wpfj wnsy fchl";
    private readonly IMapper _mapper;
    private readonly IServiceProvider _serviceProvider;
    private readonly BookingRepository _bookingRepository;
    private readonly TourRepository _tourRepository;

    public SendMailService(IMapper mapper, IServiceProvider serviceProvider)
    {
        var _repositoryFactory = serviceProvider.GetService<RepositoryFactory>();
        _mapper = mapper;
        _serviceProvider = serviceProvider;
        _bookingRepository = _repositoryFactory.BookingRepository;
        _tourRepository = _repositoryFactory.TourRepository;
    }


    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false, Stream attachmentStream = null, string attachmentName = null)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("HachuTravel", _smtpUser));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = subject;

        var bodyPart = new TextPart(isHtml ? "html" : "plain")
        {
            Text = body
        };

        if (attachmentStream != null && attachmentName != null)
        {
            var multipart = new Multipart("mixed");
            multipart.Add(bodyPart);

            var attachment = new MimePart("application", "octet-stream")
            {
                Content = new MimeContent(attachmentStream),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = attachmentName
            };

            multipart.Add(attachment);
            message.Body = multipart;
        }
        else
        {
            message.Body = bodyPart;
        }

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpUser, _smtpPass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            client.Timeout = 300000; //Thiết lập thời gian chờ cho các hoạt động của SmtpClient thành 300000 ms (tương đương với 5 phút).
        }
    }
    //send mail
    public async Task SendBookingStatusEmailAsync(string to, string bookingStatus, int bookingId)
    {
        var subject = GetSubject.GetEmailSubjectForBookingStatus(bookingStatus);
        var tourDetails = await GetTourDetailsAsync(bookingId);
        var body = GetBody.GetEmailBodyForBookingStatus(bookingStatus, tourDetails);
        await SendEmailAsync(to, subject, body, true);
    }

    public async Task<List<CustomerByTour>> GetCustomerByTour(int id)
    {
        var result = await _tourRepository.GetCustomerByTour(id);
        return result;
    }

    private string RemoveLeadingNumber(string name)
    {
        return System.Text.RegularExpressions.Regex.Replace(name, @"^\d+\.\s*", "");
    }

    public async Task<MailDTO> GetTourDetailsAsync(int bookingId)
    {
        var tour = await _bookingRepository.InfoToSendMail(bookingId);
        return tour;
    }








}
