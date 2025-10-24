using System;
using System.Collections.Generic;
using APIDirecciones.Model.ViewModels.Enums;
using System.Data;


namespace APIDirecciones.Util
{
    public enum TypeMessage
  {
    Information,
    Success,
    Warning,
    Error
  }

  public class Message
  {
    TypeMessage _typeMessage;
    string _detailMessage;

    public Message(TypeMessage typeMessage, string detailMessage)
    {
        this._detailMessage = detailMessage;
        this._typeMessage = typeMessage;
    }

    public string DetailMessage { get { return this._detailMessage; } }

    public TypeMessage TypeMessage { get { return this._typeMessage; } }
  }

  public class ResultOperation<T>
  {
    public T Result { get; set; }
    public bool Success { get; set; }
    public List<Message> Messages { get; set; }

    public ResultOperation()
    {
      this.Messages = new List<Message>();
      this.Success = false;
    }

    public ResultOperation(bool success)
    {
      this.Messages = new List<Message>();
      this.Success = success;
    }

    public void AddInformationMessage(string message)
    {
      this.Messages.Add(new Message(TypeMessage.Information,message));
    }

    public void AddSuccessMessage(string message)
    {
      this.Messages.Add(new Message(TypeMessage.Success,message));
    }

    public void AddWarningMessage(string message)
    {
      this.Messages.Add(new Message(TypeMessage.Warning,message));
    }

    public void AddErrorMessage(string message)
    {
      this.Messages.Add(new Message(TypeMessage.Error,message));
    }
  }
}