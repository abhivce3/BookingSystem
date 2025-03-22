namespace BookingSystem.Tests.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFixture;
    using BookingSystem.Application.DTOs;
    using BookingSystem.Application.Features.Member.Commands;
    using BookingSystem.Application.Features.Member.Queries;
    using BookingSystem.Service.Protos;
    using BookingSystem.Service.Services;
    using FluentAssertions;
    using Google.Protobuf;
    using Google.Protobuf.WellKnownTypes;
    using Grpc.Core;
    using MediatR;
    using Moq;
    using Xunit;

    public class MemberServiceTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly MemberService _memberService;
        private readonly Fixture _fixture;
        private readonly ServerCallContext _serverCallContext;

        public MemberServiceTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _memberService = new MemberService(_mediatorMock.Object);
            _fixture = new Fixture();
            _serverCallContext = TestServerCallContext.Create();
        }

        // ✅ Test: Import Members Successfully
        [Fact]
        public async Task ImportMembers_ShouldReturnSuccess_WhenImportSucceeds()
        {
            // Arrange
            var request = new ImportMembersRequest { FileData = ByteString.CopyFrom(new byte[] { 1, 2, 3 }) };
            var expectedResponse = new ResponseDto<bool>
            {
                Success = true,
                Message = "Members imported successfully",
                StatusCode = 200,
                Data = true
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ImportMembersCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _memberService.ImportMembers(request, _serverCallContext);

            // Assert
            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            response.Message.Should().Be("Members imported successfully");
            response.Data.Should().BeTrue();
        }

        // ✅ Test: Import Members Failure Due to Invalid Data
        [Fact]
        public async Task ImportMembers_ShouldReturnFailure_WhenImportFails()
        {
            // Arrange
            var request = new ImportMembersRequest { FileData = ByteString.Empty };

            var expectedResponse = ResponseDto<bool>.ErrorResponse("Invalid file data");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ImportMembersCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _memberService.ImportMembers(request, _serverCallContext);

            // Assert
            response.Success.Should().BeFalse();
            response.Message.Should().Be("Invalid file data");
            response.Data.Should().BeFalse();
        }

        // ✅ Test: Get Member By ID - Success
        [Fact]
        public async Task GetMemberById_ShouldReturnMember_WhenMemberExists()
        {
            // Arrange
            var request = new GetMemberByIdRequest { MemberId = 1 };

            var expectedMember = _fixture.Build<Application.DTOs.MemberDto>()
                .With(m => m.DateJoined, DateTime.UtcNow) // ✅ Ensures UTC DateTime
                .Create();

            var expectedResponse = ResponseDto<Application.DTOs.MemberDto>.SuccessResponse(expectedMember);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetMemberQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _memberService.GetMemberById(request, _serverCallContext);

            // Assert
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.MemberId.Should().Be(expectedMember.MemberId);
            response.Data.DateJoined.Should().NotBeNull();
        }

        // ✅ Test: Get Member By ID - Not Found
        [Fact]
        public async Task GetMemberById_ShouldReturnError_WhenMemberNotFound()
        {
            // Arrange
            var request = new GetMemberByIdRequest { MemberId = 999 };

            var expectedResponse = ResponseDto<Application.DTOs.MemberDto>.ErrorResponse("Member not found.");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetMemberQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var response = await _memberService.GetMemberById(request, _serverCallContext);

            // Assert
            response.Success.Should().BeFalse();
            response.Message.Should().Be("Member not found.");
            response.Data.Should().BeNull();
        }
    }

}
